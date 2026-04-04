using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SidraHub.Api.Localization;

namespace SidraHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class LocalizationController : ControllerBase
{
    private readonly IStringLocalizer<SharedResource> _localizer;

    public LocalizationController(IStringLocalizer<SharedResource> localizer)
    {
        _localizer = localizer;
    }

    [HttpGet("{language}")]
    public IActionResult GetLocalization(string language)
    {
        var normalizedLanguage = NormalizeLanguage(language);
        if (normalizedLanguage is null)
        {
            return BadRequest(new { errors = new[] { "Unsupported language." } });
        }

        var culture = new CultureInfo(normalizedLanguage);
        using var cultureScope = new LocalizationCultureScope(culture);

        var flatTranslations = _localizer
            .GetAllStrings(includeParentCultures: true)
            .ToDictionary(entry => entry.Name, entry => entry.Value);

        return Ok(new
        {
            language = normalizedLanguage,
            data = BuildNestedDictionary(flatTranslations),
            count = flatTranslations.Count,
            literalTranslations = flatTranslations
        });
    }

    private static string? NormalizeLanguage(string? language)
    {
        var normalized = language?.Trim().ToLowerInvariant();
        return normalized is "en" or "ar" ? normalized : null;
    }

    private static Dictionary<string, object> BuildNestedDictionary(IReadOnlyDictionary<string, string> source)
    {
        var result = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        foreach (var (key, value) in source)
        {
            var parts = key.Split('.', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
            {
                continue;
            }

            Dictionary<string, object> current = result;
            for (var index = 0; index < parts.Length - 1; index++)
            {
                var part = parts[index];
                if (!current.TryGetValue(part, out var nested) || nested is not Dictionary<string, object> nestedDictionary)
                {
                    nestedDictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                    current[part] = nestedDictionary;
                }

                current = nestedDictionary;
            }

            current[parts[^1]] = value;
        }

        return result;
    }

    private sealed class LocalizationCultureScope : IDisposable
    {
        private readonly CultureInfo _originalCulture;
        private readonly CultureInfo _originalUICulture;

        public LocalizationCultureScope(CultureInfo culture)
        {
            _originalCulture = CultureInfo.CurrentCulture;
            _originalUICulture = CultureInfo.CurrentUICulture;
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        public void Dispose()
        {
            CultureInfo.CurrentCulture = _originalCulture;
            CultureInfo.CurrentUICulture = _originalUICulture;
        }
    }
}

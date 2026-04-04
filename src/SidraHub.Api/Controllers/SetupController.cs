using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SidraHub.Api.Localization;

namespace SidraHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class SetupController : ControllerBase
{
    private readonly IStringLocalizer<SharedResource> _localizer;

    public SetupController(IStringLocalizer<SharedResource> localizer)
    {
        _localizer = localizer;
    }

    [HttpGet("modules")]
    public IActionResult GetModules()
    {
        var modules = new[]
        {
            _localizer["Module.Authentication"].Value,
            _localizer["Module.Services"].Value,
            _localizer["Module.Orders"].Value,
            _localizer["Module.Consultations"].Value,
            _localizer["Module.Blog"].Value,
            _localizer["Module.Providers"].Value
        };

        return Ok(new
        {
            Culture = System.Globalization.CultureInfo.CurrentUICulture.Name,
            Platform = _localizer["Platform"].Value,
            Architecture = _localizer["Architecture"].Value,
            DatabaseApproach = _localizer["DatabaseApproach"].Value,
            Modules = modules
        });
    }
}

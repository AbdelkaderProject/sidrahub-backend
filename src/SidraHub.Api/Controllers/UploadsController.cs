using Microsoft.AspNetCore.Mvc;

namespace SidraHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UploadsController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private static readonly HashSet<string> AllowedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".gif",
        ".webp",
        ".bmp"
    };

    private static readonly HashSet<string> AllowedVideoExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".mp4",
        ".webm",
        ".ogg",
        ".mov"
    };

    public UploadsController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    [HttpPost]
    [RequestSizeLimit(100 * 1024 * 1024)]
    public async Task<IActionResult> Upload([FromForm] UploadFileRequest request, CancellationToken cancellationToken)
    {
        if (request.File is null || request.File.Length == 0)
        {
            return BadRequest(new { message = "No file was uploaded." });
        }

        var extension = Path.GetExtension(request.File.FileName);
        if (string.IsNullOrWhiteSpace(extension) || (!AllowedImageExtensions.Contains(extension) && !AllowedVideoExtensions.Contains(extension)))
        {
            return BadRequest(new { message = "Only image and video files are allowed." });
        }

        var folder = SanitizeFolder(request.Folder);
        var webRootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
        var uploadsFolderPath = Path.Combine(webRootPath, "uploads", folder);
        Directory.CreateDirectory(uploadsFolderPath);

        var fileName = $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
        var physicalPath = Path.Combine(uploadsFolderPath, fileName);

        await using (var stream = System.IO.File.Create(physicalPath))
        {
            await request.File.CopyToAsync(stream, cancellationToken);
        }

        var publicPath = $"/uploads/{folder}/{fileName}";
        var publicUrl = $"{Request.Scheme}://{Request.Host}{publicPath}";

        return Ok(new { url = publicUrl, path = publicPath });
    }

    private static string SanitizeFolder(string? folder)
    {
        if (string.IsNullOrWhiteSpace(folder))
        {
            return "general";
        }

        var cleaned = new string(folder
            .Trim()
            .ToLowerInvariant()
            .Select(ch => char.IsLetterOrDigit(ch) || ch is '-' or '_' ? ch : '-')
            .ToArray())
            .Trim('-');

        return string.IsNullOrWhiteSpace(cleaned) ? "general" : cleaned;
    }

    public sealed class UploadFileRequest
    {
        public IFormFile? File { get; set; }
        public string? Folder { get; set; }
    }
}

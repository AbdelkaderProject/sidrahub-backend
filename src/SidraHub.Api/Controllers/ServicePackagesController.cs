using Microsoft.AspNetCore.Mvc;
using SidraHub.Application.Services.ServicePackages;

namespace SidraHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ServicePackagesController : ControllerBase
{
    private readonly IServicePackageService _servicePackageService;

    public ServicePackagesController(IServicePackageService servicePackageService)
    {
        _servicePackageService = servicePackageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var packages = await _servicePackageService.GetAllAsync(cancellationToken);
        return Ok(packages);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var packageItem = await _servicePackageService.GetByIdAsync(id, cancellationToken);
        return packageItem is null ? NotFound() : Ok(packageItem);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UpsertServicePackageRequest request, CancellationToken cancellationToken)
    {
        var packageItem = await _servicePackageService.CreateAsync(request, cancellationToken);
        return packageItem is null ? BadRequest("Invalid ServiceId.") : CreatedAtAction(nameof(GetById), new { id = packageItem.Id }, packageItem);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpsertServicePackageRequest request, CancellationToken cancellationToken)
    {
        var updated = await _servicePackageService.UpdateAsync(id, request, cancellationToken);
        return updated ? NoContent() : BadRequest("Entity not found or invalid ServiceId.");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _servicePackageService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}

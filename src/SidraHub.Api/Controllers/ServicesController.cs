using Microsoft.AspNetCore.Mvc;
using SidraHub.Application.Services.Services;

namespace SidraHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ServicesController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public ServicesController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var services = await _serviceService.GetAllAsync(includeDeleted, cancellationToken);
        return Ok(services);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var service = await _serviceService.GetByIdAsync(id, cancellationToken);
        return service is null ? NotFound() : Ok(service);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UpsertServiceRequest request, CancellationToken cancellationToken)
    {
        var service = await _serviceService.CreateAsync(request, cancellationToken);
        return service is null ? BadRequest("Invalid ServiceCategoryId.") : CreatedAtAction(nameof(GetById), new { id = service.Id }, service);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpsertServiceRequest request, CancellationToken cancellationToken)
    {
        var updated = await _serviceService.UpdateAsync(id, request, cancellationToken);
        return updated ? NoContent() : BadRequest("Entity not found or invalid ServiceCategoryId.");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _serviceService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPost("{id:int}/restore")]
    public async Task<IActionResult> Restore(int id, CancellationToken cancellationToken)
    {
        var restored = await _serviceService.RestoreAsync(id, cancellationToken);
        return restored is null ? NotFound() : Ok(restored);
    }
}

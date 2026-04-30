using Microsoft.AspNetCore.Mvc;
using SidraHub.Application.Services.ServiceSlots;

namespace SidraHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ServiceSlotsController : ControllerBase
{
    private readonly IServiceSlotService _serviceSlotService;

    public ServiceSlotsController(IServiceSlotService serviceSlotService)
    {
        _serviceSlotService = serviceSlotService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var slots = await _serviceSlotService.GetAllAsync(cancellationToken);
        return Ok(slots);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var slot = await _serviceSlotService.GetByIdAsync(id, cancellationToken);
        return slot is null ? NotFound() : Ok(slot);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UpsertServiceSlotRequest request, CancellationToken cancellationToken)
    {
        var slot = await _serviceSlotService.CreateAsync(request, cancellationToken);
        return slot is null ? BadRequest("Invalid ServiceId or time range.") : CreatedAtAction(nameof(GetById), new { id = slot.Id }, slot);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpsertServiceSlotRequest request, CancellationToken cancellationToken)
    {
        var updated = await _serviceSlotService.UpdateAsync(id, request, cancellationToken);
        return updated ? NoContent() : BadRequest("Entity not found, invalid ServiceId, or invalid time range.");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _serviceSlotService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}

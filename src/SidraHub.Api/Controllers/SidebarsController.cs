using Microsoft.AspNetCore.Mvc;
using SidraHub.Application.Services.Sidebars;

namespace SidraHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class SidebarsController : ControllerBase
{
    private readonly ISidebarService _sidebarService;

    public SidebarsController(ISidebarService sidebarService)
    {
        _sidebarService = sidebarService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var sidebars = await _sidebarService.GetAllAsync(cancellationToken);
        return Ok(sidebars);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var sidebar = await _sidebarService.GetByIdAsync(id, cancellationToken);
        return sidebar is null ? NotFound() : Ok(sidebar);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UpsertSidebarRequest request, CancellationToken cancellationToken)
    {
        var sidebar = await _sidebarService.CreateAsync(request, cancellationToken);
        return sidebar is null ? BadRequest("Invalid ServiceId.") : CreatedAtAction(nameof(GetById), new { id = sidebar.Id }, sidebar);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpsertSidebarRequest request, CancellationToken cancellationToken)
    {
        var updated = await _sidebarService.UpdateAsync(id, request, cancellationToken);
        return updated ? NoContent() : BadRequest("Entity not found or invalid ServiceId.");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _sidebarService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}

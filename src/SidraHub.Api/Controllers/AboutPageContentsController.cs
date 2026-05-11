using Microsoft.AspNetCore.Mvc;
using SidraHub.Application.Services.AboutPageContents;

namespace SidraHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AboutPageContentsController : ControllerBase
{
    private readonly IAboutPageContentService _aboutPageContentService;

    public AboutPageContentsController(IAboutPageContentService aboutPageContentService)
    {
        _aboutPageContentService = aboutPageContentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var records = await _aboutPageContentService.GetAllAsync(cancellationToken);
        return Ok(records);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var record = await _aboutPageContentService.GetByIdAsync(id, cancellationToken);
        return record is null ? NotFound() : Ok(record);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UpsertAboutPageContentRequest request, CancellationToken cancellationToken)
    {
        var record = await _aboutPageContentService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpsertAboutPageContentRequest request, CancellationToken cancellationToken)
    {
        var updated = await _aboutPageContentService.UpdateAsync(id, request, cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _aboutPageContentService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}

using Microsoft.AspNetCore.Mvc;
using SidraHub.Application.Services.Partners;

namespace SidraHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PartnersController : ControllerBase
{
    private readonly IPartnerService _partnerService;

    public PartnersController(IPartnerService partnerService)
    {
        _partnerService = partnerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var partners = await _partnerService.GetAllAsync(cancellationToken);
        return Ok(partners);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var partner = await _partnerService.GetByIdAsync(id, cancellationToken);
        return partner is null ? NotFound() : Ok(partner);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UpsertPartnerRequest request, CancellationToken cancellationToken)
    {
        var partner = await _partnerService.CreateAsync(request, cancellationToken);
        return partner is null ? BadRequest("Invalid CompanyProfileId.") : CreatedAtAction(nameof(GetById), new { id = partner.Id }, partner);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpsertPartnerRequest request, CancellationToken cancellationToken)
    {
        var updated = await _partnerService.UpdateAsync(id, request, cancellationToken);
        return updated ? NoContent() : BadRequest("Entity not found or invalid CompanyProfileId.");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _partnerService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}

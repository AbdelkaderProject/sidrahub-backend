using Microsoft.AspNetCore.Mvc;
using SidraHub.Application.Services.CompanyProfiles;

namespace SidraHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CompanyProfilesController : ControllerBase
{
    private readonly ICompanyProfileService _companyProfileService;

    public CompanyProfilesController(ICompanyProfileService companyProfileService)
    {
        _companyProfileService = companyProfileService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var profiles = await _companyProfileService.GetAllAsync(cancellationToken);
        return Ok(profiles);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var profile = await _companyProfileService.GetByIdAsync(id, cancellationToken);
        return profile is null ? NotFound() : Ok(profile);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UpsertCompanyProfileRequest request, CancellationToken cancellationToken)
    {
        var profile = await _companyProfileService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = profile.Id }, profile);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpsertCompanyProfileRequest request, CancellationToken cancellationToken)
    {
        var updated = await _companyProfileService.UpdateAsync(id, request, cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _companyProfileService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}

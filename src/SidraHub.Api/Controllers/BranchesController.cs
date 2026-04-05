using Microsoft.AspNetCore.Mvc;
using SidraHub.Application.Services.Branches;

namespace SidraHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class BranchesController : ControllerBase
{
    private readonly IBranchService _branchService;

    public BranchesController(IBranchService branchService)
    {
        _branchService = branchService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var branches = await _branchService.GetAllAsync(cancellationToken);
        return Ok(branches);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var branch = await _branchService.GetByIdAsync(id, cancellationToken);
        return branch is null ? NotFound() : Ok(branch);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UpsertBranchRequest request, CancellationToken cancellationToken)
    {
        var branch = await _branchService.CreateAsync(request, cancellationToken);
        return branch is null ? BadRequest("Invalid CompanyProfileId.") : CreatedAtAction(nameof(GetById), new { id = branch.Id }, branch);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpsertBranchRequest request, CancellationToken cancellationToken)
    {
        var updated = await _branchService.UpdateAsync(id, request, cancellationToken);
        return updated ? NoContent() : BadRequest("Entity not found or invalid CompanyProfileId.");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _branchService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}

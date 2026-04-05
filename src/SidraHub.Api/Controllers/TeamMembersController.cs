using Microsoft.AspNetCore.Mvc;
using SidraHub.Application.Services.TeamMembers;

namespace SidraHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TeamMembersController : ControllerBase
{
    private readonly ITeamMemberService _teamMemberService;

    public TeamMembersController(ITeamMemberService teamMemberService)
    {
        _teamMemberService = teamMemberService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var teamMembers = await _teamMemberService.GetAllAsync(cancellationToken);
        return Ok(teamMembers);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var teamMember = await _teamMemberService.GetByIdAsync(id, cancellationToken);
        return teamMember is null ? NotFound() : Ok(teamMember);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UpsertTeamMemberRequest request, CancellationToken cancellationToken)
    {
        var teamMember = await _teamMemberService.CreateAsync(request, cancellationToken);
        return teamMember is null ? BadRequest("Invalid CompanyProfileId.") : CreatedAtAction(nameof(GetById), new { id = teamMember.Id }, teamMember);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpsertTeamMemberRequest request, CancellationToken cancellationToken)
    {
        var updated = await _teamMemberService.UpdateAsync(id, request, cancellationToken);
        return updated ? NoContent() : BadRequest("Entity not found or invalid CompanyProfileId.");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _teamMemberService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}

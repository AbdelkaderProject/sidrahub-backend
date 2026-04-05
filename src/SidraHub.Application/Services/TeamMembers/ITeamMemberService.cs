namespace SidraHub.Application.Services.TeamMembers;

public interface ITeamMemberService
{
    Task<IReadOnlyList<TeamMemberDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TeamMemberDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TeamMemberDto?> CreateAsync(UpsertTeamMemberRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpsertTeamMemberRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

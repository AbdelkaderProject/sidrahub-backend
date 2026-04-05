namespace SidraHub.Application.Services.Branches;

public interface IBranchService
{
    Task<IReadOnlyList<BranchDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BranchDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BranchDto?> CreateAsync(UpsertBranchRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpsertBranchRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

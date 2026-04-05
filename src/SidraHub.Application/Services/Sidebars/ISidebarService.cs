namespace SidraHub.Application.Services.Sidebars;

public interface ISidebarService
{
    Task<IReadOnlyList<SidebarDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SidebarDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<SidebarDto?> CreateAsync(UpsertSidebarRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpsertSidebarRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

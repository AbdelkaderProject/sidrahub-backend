namespace SidraHub.Application.Services.Services;

public interface IServiceService
{
    Task<IReadOnlyList<ServiceDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<ServiceDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ServiceDto?> CreateAsync(UpsertServiceRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpsertServiceRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ServiceDto?> RestoreAsync(int id, CancellationToken cancellationToken = default);
}

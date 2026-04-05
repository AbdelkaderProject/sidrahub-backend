namespace SidraHub.Application.Services.ServicePackages;

public interface IServicePackageService
{
    Task<IReadOnlyList<ServicePackageDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ServicePackageDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ServicePackageDto?> CreateAsync(UpsertServicePackageRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpsertServicePackageRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

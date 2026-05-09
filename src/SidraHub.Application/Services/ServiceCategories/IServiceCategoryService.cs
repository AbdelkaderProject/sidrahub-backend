namespace SidraHub.Application.Services.ServiceCategories;

public interface IServiceCategoryService
{
    Task<IReadOnlyList<ServiceCategoryDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<ServiceCategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ServiceCategoryDto> CreateAsync(UpsertServiceCategoryRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpsertServiceCategoryRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ServiceCategoryDto?> RestoreAsync(int id, CancellationToken cancellationToken = default);
}

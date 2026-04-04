namespace SidraHub.Application.Services.ServiceCategories;

public interface IServiceCategoryService
{
    Task<IReadOnlyList<ServiceCategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);
}

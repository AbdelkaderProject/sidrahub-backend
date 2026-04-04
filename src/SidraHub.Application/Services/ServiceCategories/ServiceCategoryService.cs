using SidraHub.Application.Common.Interfaces;

namespace SidraHub.Application.Services.ServiceCategories;

public sealed class ServiceCategoryService : IServiceCategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public ServiceCategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ServiceCategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _unitOfWork.ServiceCategories.GetAllAsync(cancellationToken);

        return categories
            .OrderBy(category => category.DisplayOrder)
            .Select(category => new ServiceCategoryDto(
                category.Id,
                category.NameAr,
                category.NameEn,
                category.Icon,
                category.DisplayOrder))
            .ToList();
    }
}

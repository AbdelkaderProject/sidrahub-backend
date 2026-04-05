using SidraHub.Application.Common.Interfaces;
using SidraHub.Domain.Entities;

namespace SidraHub.Application.Services.ServiceCategories;

public sealed class ServiceCategoryService : IServiceCategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public ServiceCategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceCategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _unitOfWork.ServiceCategories.GetByIdAsync(id, cancellationToken);
        return category is null ? null : Map(category);
    }

    public async Task<IReadOnlyList<ServiceCategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _unitOfWork.ServiceCategories.GetAllAsync(cancellationToken);

        return categories
            .OrderBy(category => category.Id)
            .Select(Map)
            .ToList();
    }

    public async Task<ServiceCategoryDto> CreateAsync(UpsertServiceCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = new ServiceCategory
        {
            NameAr = request.NameAr,
            NameEn = request.NameEn
        };

        await _unitOfWork.ServiceCategories.AddAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(category);
    }

    public async Task<bool> UpdateAsync(int id, UpsertServiceCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = await _unitOfWork.ServiceCategories.GetByIdAsync(id, cancellationToken);
        if (category is null)
        {
            return false;
        }

        category.NameAr = request.NameAr;
        category.NameEn = request.NameEn;

        _unitOfWork.ServiceCategories.Update(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _unitOfWork.ServiceCategories.GetByIdAsync(id, cancellationToken);
        if (category is null)
        {
            return false;
        }

        _unitOfWork.ServiceCategories.Remove(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static ServiceCategoryDto Map(ServiceCategory category)
    {
        return new ServiceCategoryDto(
            category.Id,
            category.NameAr,
            category.NameEn);
    }
}

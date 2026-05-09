using SidraHub.Application.Common.Interfaces;
using SidraHub.Domain.Entities;

namespace SidraHub.Application.Services.Services;

public sealed class ServiceService : IServiceService
{
    private readonly IUnitOfWork _unitOfWork;

    public ServiceService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ServiceDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var services = includeDeleted
            ? await _unitOfWork.Repository<Service>().GetAllIncludingDeletedAsync(cancellationToken)
            : await _unitOfWork.Repository<Service>().GetAllAsync(cancellationToken);

        return services.OrderBy(service => service.Id).Select(Map).ToList();
    }

    public async Task<ServiceDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var service = await _unitOfWork.Repository<Service>().GetByIdAsync(id, cancellationToken);
        return service is null ? null : Map(service);
    }

    public async Task<ServiceDto?> CreateAsync(UpsertServiceRequest request, CancellationToken cancellationToken = default)
    {
        var category = await _unitOfWork.ServiceCategories.GetByIdAsync(request.ServiceCategoryId, cancellationToken);
        if (category is null)
        {
            return null;
        }

        var service = new Service
        {
            ServiceCategoryId = request.ServiceCategoryId,
            NameAr = request.NameAr,
            NameEn = request.NameEn,
            ShortDescriptionAr = request.ShortDescriptionAr,
            ShortDescriptionEn = request.ShortDescriptionEn,
            DescriptionAr = request.DescriptionAr,
            DescriptionEn = request.DescriptionEn,
            Icon = request.Icon
        };

        await _unitOfWork.Repository<Service>().AddAsync(service, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(service);
    }

    public async Task<bool> UpdateAsync(int id, UpsertServiceRequest request, CancellationToken cancellationToken = default)
    {
        var service = await _unitOfWork.Repository<Service>().GetByIdAsync(id, cancellationToken);
        if (service is null)
        {
            return false;
        }

        var category = await _unitOfWork.ServiceCategories.GetByIdAsync(request.ServiceCategoryId, cancellationToken);
        if (category is null)
        {
            return false;
        }

        service.ServiceCategoryId = request.ServiceCategoryId;
        service.NameAr = request.NameAr;
        service.NameEn = request.NameEn;
        service.ShortDescriptionAr = request.ShortDescriptionAr;
        service.ShortDescriptionEn = request.ShortDescriptionEn;
        service.DescriptionAr = request.DescriptionAr;
        service.DescriptionEn = request.DescriptionEn;
        service.Icon = request.Icon;

        _unitOfWork.Repository<Service>().Update(service);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var service = await _unitOfWork.Repository<Service>().GetByIdAsync(id, cancellationToken);
        if (service is null)
        {
            return false;
        }

        _unitOfWork.Repository<Service>().Remove(service);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<ServiceDto?> RestoreAsync(int id, CancellationToken cancellationToken = default)
    {
        var service = await _unitOfWork.Repository<Service>().GetByIdIncludingDeletedAsync(id, cancellationToken);
        if (service is null || !service.IsDeleted)
        {
            return null;
        }

        service.IsDeleted = false;
        service.DeletedBy = null;
        service.DeletedDateTime = null;

        _unitOfWork.Repository<Service>().Update(service);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(service);
    }

    private static ServiceDto Map(Service service)
    {
        return new ServiceDto(
            service.Id,
            service.ServiceCategoryId,
            service.NameAr,
            service.NameEn,
            service.ShortDescriptionAr,
            service.ShortDescriptionEn,
            service.DescriptionAr,
            service.DescriptionEn,
            service.Icon,
            service.IsDeleted);
    }
}

using SidraHub.Application.Common.Interfaces;
using SidraHub.Domain.Entities;

namespace SidraHub.Application.Services.ServicePackages;

public sealed class ServicePackageService : IServicePackageService
{
    private readonly IUnitOfWork _unitOfWork;

    public ServicePackageService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ServicePackageDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var packages = await _unitOfWork.Repository<ServicePackage>().GetAllAsync(cancellationToken);
        return packages.OrderBy(packageItem => packageItem.Id).Select(Map).ToList();
    }

    public async Task<ServicePackageDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var packageItem = await _unitOfWork.Repository<ServicePackage>().GetByIdAsync(id, cancellationToken);
        return packageItem is null ? null : Map(packageItem);
    }

    public async Task<ServicePackageDto?> CreateAsync(UpsertServicePackageRequest request, CancellationToken cancellationToken = default)
    {
        var service = await _unitOfWork.Repository<Service>().GetByIdAsync(request.ServiceId, cancellationToken);
        if (service is null)
        {
            return null;
        }

        var packageItem = new ServicePackage
        {
            ServiceId = request.ServiceId,
            NameAr = request.NameAr,
            NameEn = request.NameEn,
            Icon = request.Icon,
            CostAmount = request.CostAmount
        };

        await _unitOfWork.Repository<ServicePackage>().AddAsync(packageItem, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(packageItem);
    }

    public async Task<bool> UpdateAsync(int id, UpsertServicePackageRequest request, CancellationToken cancellationToken = default)
    {
        var packageItem = await _unitOfWork.Repository<ServicePackage>().GetByIdAsync(id, cancellationToken);
        if (packageItem is null)
        {
            return false;
        }

        var service = await _unitOfWork.Repository<Service>().GetByIdAsync(request.ServiceId, cancellationToken);
        if (service is null)
        {
            return false;
        }

        packageItem.ServiceId = request.ServiceId;
        packageItem.NameAr = request.NameAr;
        packageItem.NameEn = request.NameEn;
        packageItem.Icon = request.Icon;
        packageItem.CostAmount = request.CostAmount;

        _unitOfWork.Repository<ServicePackage>().Update(packageItem);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var packageItem = await _unitOfWork.Repository<ServicePackage>().GetByIdAsync(id, cancellationToken);
        if (packageItem is null)
        {
            return false;
        }

        _unitOfWork.Repository<ServicePackage>().Remove(packageItem);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static ServicePackageDto Map(ServicePackage packageItem)
    {
        return new ServicePackageDto(
            packageItem.Id,
            packageItem.ServiceId,
            packageItem.NameAr,
            packageItem.NameEn,
            packageItem.Icon,
            packageItem.CostAmount);
    }
}

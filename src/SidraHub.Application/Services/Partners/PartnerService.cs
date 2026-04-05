using SidraHub.Application.Common.Interfaces;
using SidraHub.Domain.Entities;

namespace SidraHub.Application.Services.Partners;

public sealed class PartnerService : IPartnerService
{
    private readonly IUnitOfWork _unitOfWork;

    public PartnerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<PartnerDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var partners = await _unitOfWork.Repository<Partner>().GetAllAsync(cancellationToken);
        return partners.OrderBy(entity => entity.Id).Select(Map).ToList();
    }

    public async Task<PartnerDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var partner = await _unitOfWork.Repository<Partner>().GetByIdAsync(id, cancellationToken);
        return partner is null ? null : Map(partner);
    }

    public async Task<PartnerDto?> CreateAsync(UpsertPartnerRequest request, CancellationToken cancellationToken = default)
    {
        var companyProfile = await _unitOfWork.Repository<CompanyProfile>().GetByIdAsync(request.CompanyProfileId, cancellationToken);
        if (companyProfile is null)
        {
            return null;
        }

        var partner = new Partner
        {
            CompanyProfileId = request.CompanyProfileId,
            NameEn = request.NameEn,
            NameAr = request.NameAr,
            Logo = request.Logo
        };

        await _unitOfWork.Repository<Partner>().AddAsync(partner, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(partner);
    }

    public async Task<bool> UpdateAsync(int id, UpsertPartnerRequest request, CancellationToken cancellationToken = default)
    {
        var partner = await _unitOfWork.Repository<Partner>().GetByIdAsync(id, cancellationToken);
        if (partner is null)
        {
            return false;
        }

        var companyProfile = await _unitOfWork.Repository<CompanyProfile>().GetByIdAsync(request.CompanyProfileId, cancellationToken);
        if (companyProfile is null)
        {
            return false;
        }

        partner.CompanyProfileId = request.CompanyProfileId;
        partner.NameEn = request.NameEn;
        partner.NameAr = request.NameAr;
        partner.Logo = request.Logo;

        _unitOfWork.Repository<Partner>().Update(partner);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var partner = await _unitOfWork.Repository<Partner>().GetByIdAsync(id, cancellationToken);
        if (partner is null)
        {
            return false;
        }

        _unitOfWork.Repository<Partner>().Remove(partner);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static PartnerDto Map(Partner entity)
    {
        return new PartnerDto(
            entity.Id,
            entity.CompanyProfileId,
            entity.NameEn,
            entity.NameAr,
            entity.Logo);
    }
}

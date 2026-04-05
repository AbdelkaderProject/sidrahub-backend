using SidraHub.Application.Common.Interfaces;
using SidraHub.Domain.Entities;

namespace SidraHub.Application.Services.CompanyProfiles;

public sealed class CompanyProfileService : ICompanyProfileService
{
    private readonly IUnitOfWork _unitOfWork;

    public CompanyProfileService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<CompanyProfileDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var profiles = await _unitOfWork.Repository<CompanyProfile>().GetAllAsync(cancellationToken);
        return profiles.OrderBy(profile => profile.Id).Select(Map).ToList();
    }

    public async Task<CompanyProfileDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var profile = await _unitOfWork.Repository<CompanyProfile>().GetByIdAsync(id, cancellationToken);
        return profile is null ? null : Map(profile);
    }

    public async Task<CompanyProfileDto> CreateAsync(UpsertCompanyProfileRequest request, CancellationToken cancellationToken = default)
    {
        var profile = new CompanyProfile
        {
            NameAr = request.NameAr,
            NameEn = request.NameEn,
            Logo = request.Logo,
            DescriptionAr = request.DescriptionAr,
            DescriptionEn = request.DescriptionEn,
            InsgramLinkStr = request.InsgramLinkStr,
            FacebookLinkStr = request.FacebookLinkStr,
            TwitterLinkStr = request.TwitterLinkStr,
            LinkdInLinkStr = request.LinkdInLinkStr,
            WhatsApp = request.WhatsApp,
            YearExperienceNo = request.YearExperienceNo,
            SuccessStoryNo = request.SuccessStoryNo,
            HappyCustomerNo = request.HappyCustomerNo,
            TeamMembersNo = request.TeamMembersNo
        };

        await _unitOfWork.Repository<CompanyProfile>().AddAsync(profile, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(profile);
    }

    public async Task<bool> UpdateAsync(int id, UpsertCompanyProfileRequest request, CancellationToken cancellationToken = default)
    {
        var profile = await _unitOfWork.Repository<CompanyProfile>().GetByIdAsync(id, cancellationToken);
        if (profile is null)
        {
            return false;
        }

        profile.NameAr = request.NameAr;
        profile.NameEn = request.NameEn;
        profile.Logo = request.Logo;
        profile.DescriptionAr = request.DescriptionAr;
        profile.DescriptionEn = request.DescriptionEn;
        profile.InsgramLinkStr = request.InsgramLinkStr;
        profile.FacebookLinkStr = request.FacebookLinkStr;
        profile.TwitterLinkStr = request.TwitterLinkStr;
        profile.LinkdInLinkStr = request.LinkdInLinkStr;
        profile.WhatsApp = request.WhatsApp;
        profile.YearExperienceNo = request.YearExperienceNo;
        profile.SuccessStoryNo = request.SuccessStoryNo;
        profile.HappyCustomerNo = request.HappyCustomerNo;
        profile.TeamMembersNo = request.TeamMembersNo;

        _unitOfWork.Repository<CompanyProfile>().Update(profile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var profile = await _unitOfWork.Repository<CompanyProfile>().GetByIdAsync(id, cancellationToken);
        if (profile is null)
        {
            return false;
        }

        _unitOfWork.Repository<CompanyProfile>().Remove(profile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static CompanyProfileDto Map(CompanyProfile profile)
    {
        return new CompanyProfileDto(
            profile.Id,
            profile.NameAr,
            profile.NameEn,
            profile.Logo,
            profile.DescriptionAr,
            profile.DescriptionEn,
            profile.InsgramLinkStr,
            profile.FacebookLinkStr,
            profile.TwitterLinkStr,
            profile.LinkdInLinkStr,
            profile.WhatsApp,
            profile.YearExperienceNo,
            profile.SuccessStoryNo,
            profile.HappyCustomerNo,
            profile.TeamMembersNo);
    }
}

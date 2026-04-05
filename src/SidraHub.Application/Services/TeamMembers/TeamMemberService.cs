using SidraHub.Application.Common.Interfaces;
using SidraHub.Domain.Entities;

namespace SidraHub.Application.Services.TeamMembers;

public sealed class TeamMemberService : ITeamMemberService
{
    private readonly IUnitOfWork _unitOfWork;

    public TeamMemberService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<TeamMemberDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var teamMembers = await _unitOfWork.Repository<TeamMember>().GetAllAsync(cancellationToken);
        return teamMembers.OrderBy(entity => entity.Id).Select(Map).ToList();
    }

    public async Task<TeamMemberDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var teamMember = await _unitOfWork.Repository<TeamMember>().GetByIdAsync(id, cancellationToken);
        return teamMember is null ? null : Map(teamMember);
    }

    public async Task<TeamMemberDto?> CreateAsync(UpsertTeamMemberRequest request, CancellationToken cancellationToken = default)
    {
        var companyProfile = await _unitOfWork.Repository<CompanyProfile>().GetByIdAsync(request.CompanyProfileId, cancellationToken);
        if (companyProfile is null)
        {
            return null;
        }

        var teamMember = new TeamMember
        {
            CompanyProfileId = request.CompanyProfileId,
            NameEn = request.NameEn,
            NameAr = request.NameAr,
            DescriptionEn = request.DescriptionEn,
            DescriptionAr = request.DescriptionAr,
            InsgramLinkStr = request.InsgramLinkStr,
            FacebookLinkStr = request.FacebookLinkStr,
            TwitterLinkStr = request.TwitterLinkStr,
            LinkdInLinkStr = request.LinkdInLinkStr,
            WhatsApp = request.WhatsApp
        };

        await _unitOfWork.Repository<TeamMember>().AddAsync(teamMember, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(teamMember);
    }

    public async Task<bool> UpdateAsync(int id, UpsertTeamMemberRequest request, CancellationToken cancellationToken = default)
    {
        var teamMember = await _unitOfWork.Repository<TeamMember>().GetByIdAsync(id, cancellationToken);
        if (teamMember is null)
        {
            return false;
        }

        var companyProfile = await _unitOfWork.Repository<CompanyProfile>().GetByIdAsync(request.CompanyProfileId, cancellationToken);
        if (companyProfile is null)
        {
            return false;
        }

        teamMember.CompanyProfileId = request.CompanyProfileId;
        teamMember.NameEn = request.NameEn;
        teamMember.NameAr = request.NameAr;
        teamMember.DescriptionEn = request.DescriptionEn;
        teamMember.DescriptionAr = request.DescriptionAr;
        teamMember.InsgramLinkStr = request.InsgramLinkStr;
        teamMember.FacebookLinkStr = request.FacebookLinkStr;
        teamMember.TwitterLinkStr = request.TwitterLinkStr;
        teamMember.LinkdInLinkStr = request.LinkdInLinkStr;
        teamMember.WhatsApp = request.WhatsApp;

        _unitOfWork.Repository<TeamMember>().Update(teamMember);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var teamMember = await _unitOfWork.Repository<TeamMember>().GetByIdAsync(id, cancellationToken);
        if (teamMember is null)
        {
            return false;
        }

        _unitOfWork.Repository<TeamMember>().Remove(teamMember);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static TeamMemberDto Map(TeamMember entity)
    {
        return new TeamMemberDto(
            entity.Id,
            entity.CompanyProfileId,
            entity.NameEn,
            entity.NameAr,
            entity.DescriptionEn,
            entity.DescriptionAr,
            entity.InsgramLinkStr,
            entity.FacebookLinkStr,
            entity.TwitterLinkStr,
            entity.LinkdInLinkStr,
            entity.WhatsApp);
    }
}

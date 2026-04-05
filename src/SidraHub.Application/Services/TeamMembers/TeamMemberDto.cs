namespace SidraHub.Application.Services.TeamMembers;

public sealed record TeamMemberDto(
    int Id,
    int CompanyProfileId,
    string NameEn,
    string NameAr,
    string DescriptionEn,
    string DescriptionAr,
    string? InsgramLinkStr,
    string? FacebookLinkStr,
    string? TwitterLinkStr,
    string? LinkdInLinkStr,
    string? WhatsApp);

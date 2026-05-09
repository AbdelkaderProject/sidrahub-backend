namespace SidraHub.Application.Services.TeamMembers;

public sealed record TeamMemberDto(
    int Id,
    string NameEn,
    string NameAr,
    string DescriptionEn,
    string DescriptionAr,
    string? Image,
    string? InsgramLinkStr,
    string? FacebookLinkStr,
    string? TwitterLinkStr,
    string? LinkdInLinkStr,
    string? WhatsApp);

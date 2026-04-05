namespace SidraHub.Application.Services.CompanyProfiles;

public sealed record CompanyProfileDto(
    int Id,
    string NameAr,
    string NameEn,
    string? Logo,
    string DescriptionAr,
    string DescriptionEn,
    string? InsgramLinkStr,
    string? FacebookLinkStr,
    string? TwitterLinkStr,
    string? LinkdInLinkStr,
    string? WhatsApp,
    int YearExperienceNo,
    int SuccessStoryNo,
    int HappyCustomerNo,
    int TeamMembersNo);

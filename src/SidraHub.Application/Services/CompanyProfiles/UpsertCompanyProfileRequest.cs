namespace SidraHub.Application.Services.CompanyProfiles;

public sealed class UpsertCompanyProfileRequest
{
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? Logo { get; set; }
    public string DescriptionAr { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string? InsgramLinkStr { get; set; }
    public string? FacebookLinkStr { get; set; }
    public string? TwitterLinkStr { get; set; }
    public string? LinkdInLinkStr { get; set; }
    public string? WhatsApp { get; set; }
    public int YearExperienceNo { get; set; }
    public int SuccessStoryNo { get; set; }
    public int HappyCustomerNo { get; set; }
    public int TeamMembersNo { get; set; }
}

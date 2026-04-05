namespace SidraHub.Application.Services.TeamMembers;

public sealed class UpsertTeamMemberRequest
{
    public int CompanyProfileId { get; set; }
    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string DescriptionAr { get; set; } = string.Empty;
    public string? InsgramLinkStr { get; set; }
    public string? FacebookLinkStr { get; set; }
    public string? TwitterLinkStr { get; set; }
    public string? LinkdInLinkStr { get; set; }
    public string? WhatsApp { get; set; }
}

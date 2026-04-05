using SidraHub.Domain.Common;

namespace SidraHub.Domain.Entities;

public sealed class TeamMember : BaseEntity
{
    public int CompanyProfileId { get; set; }
    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string DescriptionAr { get; set; } = string.Empty;
    public string? InsgramLinkStr { get; set; } = string.Empty;
    public string? FacebookLinkStr { get; set; } = string.Empty;
    public string? TwitterLinkStr { get; set; } = string.Empty;
    public string? LinkdInLinkStr { get; set; } = string.Empty;
    public string? WhatsApp { get; set; } = string.Empty;

    public CompanyProfile CompanyProfile { get; set; } = null!;
}

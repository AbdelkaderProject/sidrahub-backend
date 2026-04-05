using SidraHub.Domain.Common;

namespace SidraHub.Domain.Entities;

public sealed class Partner : BaseEntity
{
    public int CompanyProfileId { get; set; }
    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string? Logo { get; set; }

    public CompanyProfile CompanyProfile { get; set; } = null!;
}

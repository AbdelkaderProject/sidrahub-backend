using SidraHub.Domain.Common;

namespace SidraHub.Domain.Entities;

public sealed class Sidebar : BaseEntity
{
    public string TitleEn { get; set; } = string.Empty;
    public string TitleAr { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string DescriptionAr { get; set; } = string.Empty;
    public string? Image { get; set; }
    public int ServiceId { get; set; }

    public Service Service { get; set; } = null!;
}

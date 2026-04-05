using SidraHub.Domain.Common;

namespace SidraHub.Domain.Entities;

public sealed class ServicePackage : BaseEntity
{
    public int ServiceId { get; set; }
    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public decimal CostAmount { get; set; }

    public Service Service { get; set; } = null!;
}

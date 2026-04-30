using SidraHub.Domain.Common;

namespace SidraHub.Domain.Entities;

public sealed class Service : BaseEntity
{
    public int ServiceCategoryId { get; set; }
    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string ShortDescriptionEn { get; set; } = string.Empty;
    public string ShortDescriptionAr { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string DescriptionAr { get; set; } = string.Empty;
    public string? Icon { get; set; }

    public ServiceCategory ServiceCategory { get; set; } = null!;
    public ICollection<Sidebar> Sidebars { get; set; } = new List<Sidebar>();
    public ICollection<ServicePackage> ServicePackages { get; set; } = new List<ServicePackage>();
    public ICollection<ServiceSlot> ServiceSlots { get; set; } = new List<ServiceSlot>();
    public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
}

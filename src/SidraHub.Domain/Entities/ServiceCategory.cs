using SidraHub.Domain.Common;

namespace SidraHub.Domain.Entities;

public sealed class ServiceCategory : BaseEntity
{
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int DisplayOrder { get; set; }

    public ICollection<Service> Services { get; set; } = new List<Service>();
}
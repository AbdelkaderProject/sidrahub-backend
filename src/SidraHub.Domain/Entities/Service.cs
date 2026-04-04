using SidraHub.Domain.Common;

namespace SidraHub.Domain.Entities;

public sealed class Service : BaseEntity
{
    public Guid CategoryId { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public int EstimatedDays { get; set; }
    public bool IsActive { get; set; } = true;

    public ServiceCategory Category { get; set; } = null!;
    public ICollection<OrderService> OrderServices { get; set; } = new List<OrderService>();
    public ICollection<ServiceProvider> ServiceProviders { get; set; } = new List<ServiceProvider>();
    public ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();
}
using SidraHub.Domain.Common;

namespace SidraHub.Domain.Entities;

public sealed class OrderService : BaseEntity
{
    public Guid OrderId { get; set; }
    public Guid ServiceId { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal PriceAtTime { get; set; }

    public Order Order { get; set; } = null!;
    public Service Service { get; set; } = null!;
}
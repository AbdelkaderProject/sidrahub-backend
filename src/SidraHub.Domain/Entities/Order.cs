using SidraHub.Domain.Common;
using SidraHub.Domain.Enums;

namespace SidraHub.Domain.Entities;

public sealed class Order : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    public ICollection<OrderService> OrderServices { get; set; } = new List<OrderService>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}

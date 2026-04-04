using SidraHub.Domain.Common;

namespace SidraHub.Domain.Entities;

public sealed class Review : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public Guid OrderId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Order Order { get; set; } = null!;
}
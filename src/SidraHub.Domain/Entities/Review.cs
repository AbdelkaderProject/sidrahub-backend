using SidraHub.Domain.Common;

namespace SidraHub.Domain.Entities;

public sealed class Review : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

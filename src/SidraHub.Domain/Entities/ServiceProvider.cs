using SidraHub.Domain.Common;

namespace SidraHub.Domain.Entities;

public sealed class ServiceProvider : BaseEntity
{
    public Guid ServiceId { get; set; }
    public Guid ProviderId { get; set; }

    public Service Service { get; set; } = null!;
    public Provider Provider { get; set; } = null!;
}
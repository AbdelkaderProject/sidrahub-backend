using SidraHub.Domain.Common;

namespace SidraHub.Domain.Entities;

public sealed class ServiceSlot : BaseEntity
{
    public int ServiceId { get; set; }
    public DateOnly Day { get; set; }
    public TimeOnly TimeFrom { get; set; }
    public TimeOnly TimeTo { get; set; }
    public bool IsAvailable { get; set; } = true;

    public Service Service { get; set; } = null!;
}

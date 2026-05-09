using SidraHub.Domain.Common;
using SidraHub.Domain.Enums;

namespace SidraHub.Domain.Entities;

public sealed class ServiceRequest : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string? UserId { get; set; }
    public int ServiceId { get; set; }
    public int ServiceSlotId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly RequestDate { get; set; }
    public TimeOnly RequestTime { get; set; }
    public ServiceRequestStatus RequestStatus { get; set; } = ServiceRequestStatus.Submit;

    public Service Service { get; set; } = null!;
    public ServiceSlot ServiceSlot { get; set; } = null!;
}

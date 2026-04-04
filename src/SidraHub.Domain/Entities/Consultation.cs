using SidraHub.Domain.Common;
using SidraHub.Domain.Enums;

namespace SidraHub.Domain.Entities;

public sealed class Consultation : BaseEntity
{
    public string? UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Guid ServiceId { get; set; }
    public ConsultationStatus Status { get; set; } = ConsultationStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Service Service { get; set; } = null!;
}
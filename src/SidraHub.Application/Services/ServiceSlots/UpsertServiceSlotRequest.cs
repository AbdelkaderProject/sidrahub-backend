namespace SidraHub.Application.Services.ServiceSlots;

public sealed class UpsertServiceSlotRequest
{
    public int ServiceId { get; set; }
    public DateOnly Day { get; set; }
    public TimeOnly TimeFrom { get; set; }
    public TimeOnly TimeTo { get; set; }
    public bool IsAvailable { get; set; } = true;
}

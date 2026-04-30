namespace SidraHub.Application.Services.ServiceSlots;

public sealed record ServiceSlotDto(
    int Id,
    int ServiceId,
    DateOnly Day,
    TimeOnly TimeFrom,
    TimeOnly TimeTo,
    bool IsAvailable);

namespace SidraHub.Application.Services.ServiceRequests;

public sealed record ServiceRequestDto(
    int Id,
    string Code,
    int ServiceId,
    string ServiceNameAr,
    string ServiceNameEn,
    int ServiceSlotId,
    string CustomerName,
    string CustomerEmail,
    string CustomerPhone,
    string Description,
    DateOnly RequestDate,
    TimeOnly RequestTime,
    int RequestStatus,
    string RequestStatusLabel);

namespace SidraHub.Application.Services.ServicePackages;

public sealed record ServicePackageDto(
    int Id,
    int ServiceId,
    string NameAr,
    string NameEn,
    string? Icon,
    decimal CostAmount);

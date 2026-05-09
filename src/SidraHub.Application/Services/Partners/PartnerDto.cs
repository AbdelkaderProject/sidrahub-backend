namespace SidraHub.Application.Services.Partners;

public sealed record PartnerDto(
    int Id,
    string NameEn,
    string NameAr,
    string? Logo);

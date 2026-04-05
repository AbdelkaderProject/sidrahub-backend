namespace SidraHub.Application.Services.Partners;

public sealed record PartnerDto(
    int Id,
    int CompanyProfileId,
    string NameEn,
    string NameAr,
    string? Logo);

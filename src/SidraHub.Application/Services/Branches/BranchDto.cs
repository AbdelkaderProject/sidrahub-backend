namespace SidraHub.Application.Services.Branches;

public sealed record BranchDto(
    int Id,
    int CompanyProfileId,
    string NameEn,
    string NameAr,
    string AddressEn,
    string AddressAr,
    string PhoneNumber);

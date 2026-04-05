namespace SidraHub.Application.Services.Branches;

public sealed class UpsertBranchRequest
{
    public int CompanyProfileId { get; set; }
    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string AddressEn { get; set; } = string.Empty;
    public string AddressAr { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

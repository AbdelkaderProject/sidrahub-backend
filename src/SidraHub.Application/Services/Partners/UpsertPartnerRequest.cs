namespace SidraHub.Application.Services.Partners;

public sealed class UpsertPartnerRequest
{
    public int CompanyProfileId { get; set; }
    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string? Logo { get; set; }
}

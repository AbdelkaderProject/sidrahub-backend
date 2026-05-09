namespace SidraHub.Application.Services.Partners;

public sealed class UpsertPartnerRequest
{
    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string? Logo { get; set; }
}

namespace SidraHub.Application.Services.ServicePackages;

public sealed class UpsertServicePackageRequest
{
    public int ServiceId { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public decimal CostAmount { get; set; }
}

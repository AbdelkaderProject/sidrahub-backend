namespace SidraHub.Application.Services.Services;

public sealed class UpsertServiceRequest
{
    public int ServiceCategoryId { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string ShortDescriptionAr { get; set; } = string.Empty;
    public string ShortDescriptionEn { get; set; } = string.Empty;
    public string DescriptionAr { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string? Icon { get; set; }
}

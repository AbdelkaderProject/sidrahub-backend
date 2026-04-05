namespace SidraHub.Application.Services.Sidebars;

public sealed class UpsertSidebarRequest
{
    public int ServiceId { get; set; }
    public string TitleAr { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;
    public string DescriptionAr { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string? Image { get; set; }
}

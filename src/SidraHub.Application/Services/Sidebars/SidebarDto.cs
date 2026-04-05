namespace SidraHub.Application.Services.Sidebars;

public sealed record SidebarDto(
    int Id,
    int ServiceId,
    string TitleAr,
    string TitleEn,
    string DescriptionAr,
    string DescriptionEn,
    string? Image);

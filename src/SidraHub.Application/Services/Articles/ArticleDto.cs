namespace SidraHub.Application.Services.Articles;

public sealed record ArticleDto(
    int Id,
    int? ServiceCategoryId,
    string TitleAr,
    string TitleEn,
    string ShortDescriptionAr,
    string ShortDescriptionEn,
    string DescriptionAr,
    string DescriptionEn,
    string? Image);

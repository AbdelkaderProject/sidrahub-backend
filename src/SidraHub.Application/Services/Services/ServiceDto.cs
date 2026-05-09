namespace SidraHub.Application.Services.Services;

public sealed record ServiceDto(
    int Id,
    int ServiceCategoryId,
    string NameAr,
    string NameEn,
    string ShortDescriptionAr,
    string ShortDescriptionEn,
    string DescriptionAr,
    string DescriptionEn,
    string? Icon,
    bool IsDeleted);

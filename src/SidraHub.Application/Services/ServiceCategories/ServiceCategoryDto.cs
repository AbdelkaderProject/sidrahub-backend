namespace SidraHub.Application.Services.ServiceCategories;

public sealed record ServiceCategoryDto(
    Guid Id,
    string NameAr,
    string NameEn,
    string? Icon,
    int DisplayOrder);

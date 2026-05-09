using SidraHub.Domain.Common;

namespace SidraHub.Domain.Entities;

public sealed class Article : BaseEntity
{
    public int? ServiceCategoryId { get; set; }
    public string TitleEn { get; set; } = string.Empty;
    public string TitleAr { get; set; } = string.Empty;
    public string ShortDescriptionEn { get; set; } = string.Empty;
    public string ShortDescriptionAr { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string DescriptionAr { get; set; } = string.Empty;
    public string? Image { get; set; }

    public ServiceCategory? ServiceCategory { get; set; }
    public ICollection<ArticleComment> Comments { get; set; } = new List<ArticleComment>();
}

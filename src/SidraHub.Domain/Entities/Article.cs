using SidraHub.Domain.Common;

namespace SidraHub.Domain.Entities;

public sealed class Article : BaseEntity
{
    public string TitleAr { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public DateTime? PublishedAt { get; set; }
}
using SidraHub.Domain.Common;

namespace SidraHub.Domain.Entities;

public sealed class ArticleComment : BaseEntity
{
    public int ArticleId { get; set; }
    public string CommentContent { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;

    public Article Article { get; set; } = null!;
}

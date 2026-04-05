namespace SidraHub.Application.Services.ArticleComments;

public sealed class UpsertArticleCommentRequest
{
    public int ArticleId { get; set; }
    public string CommentContent { get; set; } = string.Empty;
}

namespace SidraHub.Application.Services.ArticleComments;

public sealed record ArticleCommentDto(
    int Id,
    int ArticleId,
    string CommentContent,
    string UserId,
    string UserName);

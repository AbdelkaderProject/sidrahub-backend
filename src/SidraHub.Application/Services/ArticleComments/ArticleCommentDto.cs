namespace SidraHub.Application.Services.ArticleComments;

public sealed record ArticleCommentDto(
    int Id,
    int ArticleId,
    string ArticleTitleEn,
    string ArticleTitleAr,
    string CommentContent,
    string UserId,
    string UserName,
    int Status,
    string StatusName);

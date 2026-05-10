namespace SidraHub.Application.Services.ArticleComments;

public interface IArticleCommentService
{
    Task<IReadOnlyList<ArticleCommentDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ArticleCommentDto>> GetByArticleIdAsync(int articleId, CancellationToken cancellationToken = default);
    Task<ArticleCommentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ArticleCommentDto?> CreateAsync(UpsertArticleCommentRequest request, string userId, string userName, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpsertArticleCommentRequest request, string userId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, string userId, CancellationToken cancellationToken = default);
}

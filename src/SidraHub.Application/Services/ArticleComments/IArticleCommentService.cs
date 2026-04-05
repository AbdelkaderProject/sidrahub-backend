namespace SidraHub.Application.Services.ArticleComments;

public interface IArticleCommentService
{
    Task<IReadOnlyList<ArticleCommentDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ArticleCommentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ArticleCommentDto?> CreateAsync(UpsertArticleCommentRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpsertArticleCommentRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

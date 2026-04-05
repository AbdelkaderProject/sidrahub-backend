namespace SidraHub.Application.Services.Articles;

public interface IArticleService
{
    Task<IReadOnlyList<ArticleDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ArticleDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ArticleDto> CreateAsync(UpsertArticleRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpsertArticleRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

using SidraHub.Application.Common.Interfaces;
using SidraHub.Domain.Entities;

namespace SidraHub.Application.Services.ArticleComments;

public sealed class ArticleCommentService : IArticleCommentService
{
    private readonly IUnitOfWork _unitOfWork;

    public ArticleCommentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ArticleCommentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var comments = await _unitOfWork.Repository<ArticleComment>().GetAllAsync(cancellationToken);
        return comments.OrderBy(comment => comment.Id).Select(Map).ToList();
    }

    public async Task<IReadOnlyList<ArticleCommentDto>> GetByArticleIdAsync(int articleId, CancellationToken cancellationToken = default)
    {
        var comments = await _unitOfWork.Repository<ArticleComment>().FindAsync(comment => comment.ArticleId == articleId, cancellationToken);
        return comments.OrderBy(comment => comment.Id).Select(Map).ToList();
    }

    public async Task<ArticleCommentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var comment = await _unitOfWork.Repository<ArticleComment>().GetByIdAsync(id, cancellationToken);
        return comment is null ? null : Map(comment);
    }

    public async Task<ArticleCommentDto?> CreateAsync(
        UpsertArticleCommentRequest request,
        string userId,
        string userName,
        CancellationToken cancellationToken = default)
    {
        var article = await _unitOfWork.Repository<Article>().GetByIdAsync(request.ArticleId, cancellationToken);
        if (article is null)
        {
            return null;
        }

        var comment = new ArticleComment
        {
            ArticleId = request.ArticleId,
            CommentContent = request.CommentContent,
            UserId = userId,
            UserName = userName
        };

        await _unitOfWork.Repository<ArticleComment>().AddAsync(comment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(comment);
    }

    public async Task<bool> UpdateAsync(int id, UpsertArticleCommentRequest request, string userId, CancellationToken cancellationToken = default)
    {
        var comment = await _unitOfWork.Repository<ArticleComment>().GetByIdAsync(id, cancellationToken);
        if (comment is null || !string.Equals(comment.UserId, userId, StringComparison.Ordinal))
        {
            return false;
        }

        var article = await _unitOfWork.Repository<Article>().GetByIdAsync(request.ArticleId, cancellationToken);
        if (article is null)
        {
            return false;
        }

        comment.ArticleId = request.ArticleId;
        comment.CommentContent = request.CommentContent;

        _unitOfWork.Repository<ArticleComment>().Update(comment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(int id, string userId, CancellationToken cancellationToken = default)
    {
        var comment = await _unitOfWork.Repository<ArticleComment>().GetByIdAsync(id, cancellationToken);
        if (comment is null || !string.Equals(comment.UserId, userId, StringComparison.Ordinal))
        {
            return false;
        }

        _unitOfWork.Repository<ArticleComment>().Remove(comment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static ArticleCommentDto Map(ArticleComment comment)
    {
        return new ArticleCommentDto(comment.Id, comment.ArticleId, comment.CommentContent, comment.UserId, comment.UserName);
    }
}

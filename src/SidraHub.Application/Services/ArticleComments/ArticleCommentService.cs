using Microsoft.EntityFrameworkCore;
using SidraHub.Application.Common.Interfaces;
using SidraHub.Domain.Entities;
using SidraHub.Domain.Enums;

namespace SidraHub.Application.Services.ArticleComments;

public sealed class ArticleCommentService : IArticleCommentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApplicationDbContext _context;

    public ArticleCommentService(IUnitOfWork unitOfWork, IApplicationDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<IReadOnlyList<ArticleCommentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var comments = await _context.ArticleComments
            .Include(c => c.Article)
            .OrderBy(c => c.Id)
            .ToListAsync(cancellationToken);
        return comments.Select(Map).ToList();
    }

    public async Task<IReadOnlyList<ArticleCommentDto>> GetByArticleIdAsync(int articleId, CancellationToken cancellationToken = default)
    {
        // Public: only approved comments
        var comments = await _context.ArticleComments
            .Include(c => c.Article)
            .Where(c => c.ArticleId == articleId && c.Status == CommentStatus.Approved)
            .OrderBy(c => c.Id)
            .ToListAsync(cancellationToken);
        return comments.Select(Map).ToList();
    }

    public async Task<ArticleCommentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var comment = await _context.ArticleComments
            .Include(c => c.Article)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
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
            UserName = userName,
            Status = CommentStatus.Pending,
            Article = article
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
        comment.Status = CommentStatus.Pending; // reset to pending after edit

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

    public async Task<bool> AdminDeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var comment = await _unitOfWork.Repository<ArticleComment>().GetByIdAsync(id, cancellationToken);
        if (comment is null)
        {
            return false;
        }

        _unitOfWork.Repository<ArticleComment>().Remove(comment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> ApproveAsync(int id, CancellationToken cancellationToken = default)
    {
        var comment = await _unitOfWork.Repository<ArticleComment>().GetByIdAsync(id, cancellationToken);
        if (comment is null)
        {
            return false;
        }

        comment.Status = CommentStatus.Approved;
        _unitOfWork.Repository<ArticleComment>().Update(comment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> RejectAsync(int id, CancellationToken cancellationToken = default)
    {
        var comment = await _unitOfWork.Repository<ArticleComment>().GetByIdAsync(id, cancellationToken);
        if (comment is null)
        {
            return false;
        }

        comment.Status = CommentStatus.Rejected;
        _unitOfWork.Repository<ArticleComment>().Update(comment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static ArticleCommentDto Map(ArticleComment comment)
    {
        return new ArticleCommentDto(
            comment.Id,
            comment.ArticleId,
            comment.Article?.TitleEn ?? string.Empty,
            comment.Article?.TitleAr ?? string.Empty,
            comment.CommentContent,
            comment.UserId,
            comment.UserName,
            (int)comment.Status,
            comment.Status.ToString());
    }
}

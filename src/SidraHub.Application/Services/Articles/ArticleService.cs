using SidraHub.Application.Common.Interfaces;
using SidraHub.Domain.Entities;

namespace SidraHub.Application.Services.Articles;

public sealed class ArticleService : IArticleService
{
    private readonly IUnitOfWork _unitOfWork;

    public ArticleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ArticleDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var articles = await _unitOfWork.Repository<Article>().GetAllAsync(cancellationToken);
        return articles.OrderBy(article => article.Id).Select(Map).ToList();
    }

    public async Task<ArticleDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var article = await _unitOfWork.Repository<Article>().GetByIdAsync(id, cancellationToken);
        return article is null ? null : Map(article);
    }

    public async Task<ArticleDto> CreateAsync(UpsertArticleRequest request, CancellationToken cancellationToken = default)
    {
        var article = new Article
        {
            TitleAr = request.TitleAr,
            TitleEn = request.TitleEn,
            ShortDescriptionAr = request.ShortDescriptionAr,
            ShortDescriptionEn = request.ShortDescriptionEn,
            DescriptionAr = request.DescriptionAr,
            DescriptionEn = request.DescriptionEn,
            Image = request.Image
        };

        await _unitOfWork.Repository<Article>().AddAsync(article, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(article);
    }

    public async Task<bool> UpdateAsync(int id, UpsertArticleRequest request, CancellationToken cancellationToken = default)
    {
        var article = await _unitOfWork.Repository<Article>().GetByIdAsync(id, cancellationToken);
        if (article is null)
        {
            return false;
        }

        article.TitleAr = request.TitleAr;
        article.TitleEn = request.TitleEn;
        article.ShortDescriptionAr = request.ShortDescriptionAr;
        article.ShortDescriptionEn = request.ShortDescriptionEn;
        article.DescriptionAr = request.DescriptionAr;
        article.DescriptionEn = request.DescriptionEn;
        article.Image = request.Image;

        _unitOfWork.Repository<Article>().Update(article);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var article = await _unitOfWork.Repository<Article>().GetByIdAsync(id, cancellationToken);
        if (article is null)
        {
            return false;
        }

        _unitOfWork.Repository<Article>().Remove(article);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static ArticleDto Map(Article article)
    {
        return new ArticleDto(
            article.Id,
            article.TitleAr,
            article.TitleEn,
            article.ShortDescriptionAr,
            article.ShortDescriptionEn,
            article.DescriptionAr,
            article.DescriptionEn,
            article.Image);
    }
}

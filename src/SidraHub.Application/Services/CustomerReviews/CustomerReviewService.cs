using SidraHub.Application.Common.Interfaces;
using SidraHub.Domain.Entities;

namespace SidraHub.Application.Services.CustomerReviews;

public sealed class CustomerReviewService : ICustomerReviewService
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerReviewService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<CustomerReviewDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var reviews = await _unitOfWork.Repository<CustomerReview>().GetAllAsync(cancellationToken);
        return reviews.OrderBy(review => review.Id).Select(Map).ToList();
    }

    public async Task<CustomerReviewDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var review = await _unitOfWork.Repository<CustomerReview>().GetByIdAsync(id, cancellationToken);
        return review is null ? null : Map(review);
    }

    public async Task<CustomerReviewDto> CreateAsync(UpsertCustomerReviewRequest request, CancellationToken cancellationToken = default)
    {
        var review = new CustomerReview
        {
            NameAr = request.NameAr,
            NameEn = request.NameEn,
            OpinionAr = request.OpinionAr,
            OpinionEn = request.OpinionEn,
            URLStr = request.URLStr
        };

        await _unitOfWork.Repository<CustomerReview>().AddAsync(review, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(review);
    }

    public async Task<bool> UpdateAsync(int id, UpsertCustomerReviewRequest request, CancellationToken cancellationToken = default)
    {
        var review = await _unitOfWork.Repository<CustomerReview>().GetByIdAsync(id, cancellationToken);
        if (review is null)
        {
            return false;
        }

        review.NameAr = request.NameAr;
        review.NameEn = request.NameEn;
        review.OpinionAr = request.OpinionAr;
        review.OpinionEn = request.OpinionEn;
        review.URLStr = request.URLStr;

        _unitOfWork.Repository<CustomerReview>().Update(review);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var review = await _unitOfWork.Repository<CustomerReview>().GetByIdAsync(id, cancellationToken);
        if (review is null)
        {
            return false;
        }

        _unitOfWork.Repository<CustomerReview>().Remove(review);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static CustomerReviewDto Map(CustomerReview review)
    {
        return new CustomerReviewDto(
            review.Id,
            review.NameAr,
            review.NameEn,
            review.OpinionAr,
            review.OpinionEn,
            review.URLStr);
    }
}

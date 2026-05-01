namespace SidraHub.Application.Services.CustomerReviews;

public interface ICustomerReviewService
{
    Task<IReadOnlyList<CustomerReviewDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CustomerReviewDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CustomerReviewDto> CreateAsync(UpsertCustomerReviewRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpsertCustomerReviewRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

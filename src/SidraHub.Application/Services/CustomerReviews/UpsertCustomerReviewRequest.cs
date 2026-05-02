namespace SidraHub.Application.Services.CustomerReviews;

public sealed class UpsertCustomerReviewRequest
{
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string OpinionAr { get; set; } = string.Empty;
    public string OpinionEn { get; set; } = string.Empty;
    public string? URLStr { get; set; }
}

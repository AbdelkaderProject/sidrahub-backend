namespace SidraHub.Application.Services.CustomerReviews;

public sealed record CustomerReviewDto(
    int Id,
    string NameAr,
    string NameEn,
    string OpinionAr,
    string OpinionEn,
    string? URLStr);

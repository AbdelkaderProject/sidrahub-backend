using SidraHub.Domain.Common;

namespace SidraHub.Domain.Entities;

public sealed class CustomerReview : BaseEntity
{
    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string OpinionEn { get; set; } = string.Empty;
    public string OpinionAr { get; set; } = string.Empty;
    public string? URLStr { get; set; }
}

using SidraHub.Domain.Common;

namespace SidraHub.Domain.Entities;

public sealed class Customer : BaseEntity
{
    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string? Logo { get; set; }
}

using SidraHub.Domain.Common;

namespace SidraHub.Domain.Entities;

public sealed class Branch : BaseEntity
{
    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string AddressEn { get; set; } = string.Empty;
    public string AddressAr { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

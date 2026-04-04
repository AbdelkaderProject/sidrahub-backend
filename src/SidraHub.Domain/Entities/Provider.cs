using SidraHub.Domain.Common;
using SidraHub.Domain.Enums;

namespace SidraHub.Domain.Entities;

public sealed class Provider : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public ProviderType Type { get; set; } = ProviderType.Other;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public ICollection<ServiceProvider> ServiceProviders { get; set; } = new List<ServiceProvider>();
}
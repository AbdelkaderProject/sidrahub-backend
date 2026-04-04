using Microsoft.AspNetCore.Identity;

namespace SidraHub.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
}
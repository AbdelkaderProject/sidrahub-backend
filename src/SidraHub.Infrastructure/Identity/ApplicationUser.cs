using Microsoft.AspNetCore.Identity;

namespace SidraHub.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser
{
    public string? FullNameAr { get; set; }
    public string? Note { get; set; }
    public string? FullName
    {
        get => FullNameAr;
        set => FullNameAr = value;
    }
}

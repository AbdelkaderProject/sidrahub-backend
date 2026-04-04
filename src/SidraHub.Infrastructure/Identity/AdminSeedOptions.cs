namespace SidraHub.Infrastructure.Identity;

public sealed class AdminSeedOptions
{
    public string FullName { get; set; } = "System Admin";
    public string Email { get; set; } = "admin@sidrahub.com";
    public string Password { get; set; } = "Admin@123";
    public string PhoneNumber { get; set; } = "01000000000";
}

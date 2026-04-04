using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace SidraHub.Infrastructure.Identity;

public sealed class IdentityDataSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AdminSeedOptions _adminSeedOptions;

    public IdentityDataSeeder(
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager,
        IOptions<AdminSeedOptions> adminSeedOptions)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _adminSeedOptions = adminSeedOptions.Value;
    }

    public async Task SeedAsync()
    {
        await EnsureRoleExistsAsync(IdentityRoles.Admin);
        await EnsureRoleExistsAsync(IdentityRoles.User);

        var adminEmail = _adminSeedOptions.Email.Trim().ToLowerInvariant();
        var adminUser = await _userManager.FindByEmailAsync(adminEmail);

        if (adminUser is null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = _adminSeedOptions.FullName,
                PhoneNumber = _adminSeedOptions.PhoneNumber,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(adminUser, _adminSeedOptions.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Failed to seed admin user: {errors}");
            }
        }

        if (!await _userManager.IsInRoleAsync(adminUser, IdentityRoles.Admin))
        {
            await _userManager.AddToRoleAsync(adminUser, IdentityRoles.Admin);
        }

        if (!await _userManager.IsInRoleAsync(adminUser, IdentityRoles.User))
        {
            await _userManager.AddToRoleAsync(adminUser, IdentityRoles.User);
        }
    }

    private async Task EnsureRoleExistsAsync(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Failed to seed role '{roleName}': {errors}");
            }
        }
    }
}

using Microsoft.AspNetCore.Identity;
using SidraHub.Application.Services.Auth;
using SidraHub.Application.Services.Auth.Models;

namespace SidraHub.Infrastructure.Identity;

public sealed class AuthService : IAuthService
{
    private const string DeletedUserMarker = "__DELETED_USER__";
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(UserManager<ApplicationUser> userManager, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser is not null)
        {
            return AuthResult.Failure("Email is already registered.");
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FullName = request.FullName.Trim(),
            PhoneNumber = request.PhoneNumber.Trim(),
            EmailConfirmed = true
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            return AuthResult.Failure(createResult.Errors.Select(error => error.Description).ToArray());
        }

        await _userManager.AddToRoleAsync(user, IdentityRoles.User);

        var roles = await _userManager.GetRolesAsync(user);
        var response = _jwtTokenGenerator.GenerateToken(user, roles.ToArray());

        return AuthResult.Success(response);
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return AuthResult.Failure("Invalid email or password.");
        }

        if (user.Note?.Contains(DeletedUserMarker, StringComparison.Ordinal) == true)
        {
            return AuthResult.Failure("This account is no longer available.");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            return AuthResult.Failure("Invalid email or password.");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var response = _jwtTokenGenerator.GenerateToken(user, roles.ToArray());

        return AuthResult.Success(response);
    }
}

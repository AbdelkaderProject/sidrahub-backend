using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SidraHub.Infrastructure.Identity;

namespace SidraHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class ProfileController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserProfileDto>> GetCurrentUser(CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync();
        if (user is null)
        {
            return Unauthorized();
        }

        return Ok(Map(user));
    }

    [HttpPut("me")]
    public async Task<ActionResult<UserProfileDto>> UpdateCurrentUser(UpdateUserProfileRequest request)
    {
        var user = await GetCurrentUserAsync();
        if (user is null)
        {
            return Unauthorized();
        }

        user.FullName = request.FullName.Trim();
        user.PhoneNumber = request.PhoneNumber.Trim();

        if (!string.Equals(user.Email, request.Email.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            var setEmailResult = await _userManager.SetEmailAsync(user, request.Email.Trim());
            if (!setEmailResult.Succeeded)
            {
                return BadRequest(new { errors = setEmailResult.Errors.Select(error => error.Description).ToArray() });
            }

            var setUserNameResult = await _userManager.SetUserNameAsync(user, request.Email.Trim().ToLowerInvariant());
            if (!setUserNameResult.Succeeded)
            {
                return BadRequest(new { errors = setUserNameResult.Errors.Select(error => error.Description).ToArray() });
            }
        }

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return BadRequest(new { errors = updateResult.Errors.Select(error => error.Description).ToArray() });
        }

        return Ok(Map(user));
    }

    private async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return string.IsNullOrWhiteSpace(userId)
            ? null
            : await _userManager.FindByIdAsync(userId);
    }

    private static UserProfileDto Map(ApplicationUser user)
    {
        return new UserProfileDto
        {
            FullName = user.FullName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
        };
    }

    public sealed class UserProfileDto
    {
        public string FullName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
    }

    public sealed class UpdateUserProfileRequest
    {
        public string FullName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
    }
}

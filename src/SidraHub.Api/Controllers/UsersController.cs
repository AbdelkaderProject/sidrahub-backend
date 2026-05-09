using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SidraHub.Infrastructure.Identity;

namespace SidraHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = IdentityRoles.Admin)]
public sealed class UsersController : ControllerBase
{
    private const string DeletedUserMarker = "__DELETED_USER__";
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserAdminDto>>> GetAll(CancellationToken cancellationToken)
    {
        var usersInUserRole = await _userManager.GetUsersInRoleAsync(IdentityRoles.User);
        var users = usersInUserRole
            .OrderBy(user => user.Email)
            .ToList();

        var result = new List<UserAdminDto>(users.Count);
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(IdentityRoles.Admin, StringComparer.OrdinalIgnoreCase))
            {
                continue;
            }

            if (IsDeleted(user))
            {
                continue;
            }

            result.Add(new UserAdminDto
            {
                Id = user.Id,
                FullName = user.FullName ?? string.Empty,
                FullNameAr = user.FullNameAr ?? string.Empty,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                EmailConfirmed = user.EmailConfirmed,
                Roles = string.Join(", ", roles)
            });
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.Equals(currentUserId, id, StringComparison.Ordinal))
        {
            return BadRequest(new { errorMessage = "You cannot delete the currently logged-in user." });
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        user.Note = BuildDeletedMarker(user.Note);
        user.LockoutEnabled = true;
        user.LockoutEnd = DateTimeOffset.MaxValue;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors.Select(error => error.Description).ToArray() });
        }

        return NoContent();
    }

    [HttpPost("{id}/restore")]
    public async Task<ActionResult<UserAdminDto>> Restore(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        user.Note = RemoveDeletedMarker(user.Note);
        user.LockoutEnd = null;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors.Select(error => error.Description).ToArray() });
        }

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(new UserAdminDto
        {
            Id = user.Id,
            FullName = user.FullName ?? string.Empty,
            FullNameAr = user.FullNameAr ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            EmailConfirmed = user.EmailConfirmed,
            Roles = string.Join(", ", roles)
        });
    }

    private static bool IsDeleted(ApplicationUser user)
    {
        return user.Note?.Contains(DeletedUserMarker, StringComparison.Ordinal) == true;
    }

    private static string BuildDeletedMarker(string? note)
    {
        var originalNote = RemoveDeletedMarker(note);
        return string.IsNullOrWhiteSpace(originalNote)
            ? DeletedUserMarker
            : $"{DeletedUserMarker}|{originalNote}";
    }

    private static string? RemoveDeletedMarker(string? note)
    {
        if (string.IsNullOrWhiteSpace(note))
        {
            return note;
        }

        if (string.Equals(note, DeletedUserMarker, StringComparison.Ordinal))
        {
            return null;
        }

        var prefix = $"{DeletedUserMarker}|";
        return note.StartsWith(prefix, StringComparison.Ordinal)
            ? note[prefix.Length..]
            : note;
    }

    public sealed class UserAdminDto
    {
        public string Id { get; init; } = string.Empty;
        public string FullName { get; init; } = string.Empty;
        public string FullNameAr { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public bool EmailConfirmed { get; init; }
        public string Roles { get; init; } = string.Empty;
    }
}

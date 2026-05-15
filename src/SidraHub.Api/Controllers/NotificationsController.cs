using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SidraHub.Application.Common.Interfaces;
using SidraHub.Infrastructure.Identity;
using System.Security.Claims;

namespace SidraHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = IdentityRoles.Admin)]
public sealed class NotificationsController : ControllerBase
{
    private readonly IApplicationDbContext _context;

    public NotificationsController(IApplicationDbContext context)
    {
        _context = context;
    }

    // GET /api/Notifications?userId=xxx&limit=20
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string userId, [FromQuery] int limit = 20, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        var notifications = await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Take(limit)
            .Select(n => new
            {
                n.Id,
                n.UserId,
                n.Title,
                n.Message,
                n.IsRead,
                n.CreatedAt,
                createdDate = n.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var unreadCount = await _context.Notifications
            .CountAsync(n => n.UserId == userId && !n.IsRead, cancellationToken);

        return Ok(new
        {
            notifications,
            unreadCount,
            totalCount = notifications.Count
        });
    }

    // GET /api/Notifications/unread-count?userId=xxx
    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount([FromQuery] string userId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        var count = await _context.Notifications
            .CountAsync(n => n.UserId == userId && !n.IsRead, cancellationToken);

        return Ok(count);
    }

    // PATCH /api/Notifications/{id}/read
    [HttpPatch("{id:int}/read")]
    public async Task<IActionResult> MarkAsRead(int id, CancellationToken cancellationToken = default)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);

        if (notification is null) return NotFound();

        notification.IsRead = true;
        await _context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    // PATCH /api/Notifications/read-all
    [HttpPatch("read-all")]
    public async Task<IActionResult> MarkAllAsRead([FromBody] MarkAllReadRequest request, CancellationToken cancellationToken = default)
    {
        var userId = request.UserId ?? User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        var notifications = await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync(cancellationToken);

        foreach (var n in notifications)
            n.IsRead = true;

        await _context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    // DELETE /api/Notifications/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);

        if (notification is null) return NotFound();

        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}

public sealed record MarkAllReadRequest(string? UserId);

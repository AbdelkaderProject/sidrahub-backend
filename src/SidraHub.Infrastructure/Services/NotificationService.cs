using Microsoft.AspNetCore.Identity;
using SidraHub.Application.Common.Interfaces;
using SidraHub.Domain.Entities;
using SidraHub.Infrastructure.Identity;

namespace SidraHub.Infrastructure.Services;

public sealed class NotificationService : INotificationService
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public NotificationService(IApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task NotifyAdminsAsync(string title, string message, CancellationToken cancellationToken = default)
    {
        // Get all admin users
        var admins = await _userManager.GetUsersInRoleAsync(IdentityRoles.Admin);

        foreach (var admin in admins)
        {
            var notification = new Notification
            {
                UserId = admin.Id,
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Notifications.AddAsync(notification, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}

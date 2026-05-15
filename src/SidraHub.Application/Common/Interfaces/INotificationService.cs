namespace SidraHub.Application.Common.Interfaces;

public interface INotificationService
{
    /// <summary>Send a notification to all admin users</summary>
    Task NotifyAdminsAsync(string title, string message, CancellationToken cancellationToken = default);
}

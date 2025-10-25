using RazorReaper.Models;

namespace RazorReaper.Services.Implementations;

public class NotificationService : INotificationService
{
    public event Action<NotificationMessage>? OnNotificationAdded;
    public event Action<string>? OnNotificationRemoved;

    public void ShowSuccess(string message, int durationMs = 5000)
    {
        var notification = new NotificationMessage
        {
            Message = message,
            Type = NotificationType.Success,
            DurationMs = durationMs
        };
        OnNotificationAdded?.Invoke(notification);
    }

    public void ShowError(string message, int durationMs = 7000)
    {
        var notification = new NotificationMessage
        {
            Message = message,
            Type = NotificationType.Error,
            DurationMs = durationMs
        };
        OnNotificationAdded?.Invoke(notification);
    }

    public void ShowWarning(string message, int durationMs = 6000)
    {
        var notification = new NotificationMessage
        {
            Message = message,
            Type = NotificationType.Warning,
            DurationMs = durationMs
        };
        OnNotificationAdded?.Invoke(notification);
    }

    public void ShowInfo(string message, int durationMs = 5000)
    {
        var notification = new NotificationMessage
        {
            Message = message,
            Type = NotificationType.Info,
            DurationMs = durationMs
        };
        OnNotificationAdded?.Invoke(notification);
    }

    public void RemoveNotification(string id)
    {
        OnNotificationRemoved?.Invoke(id);
    }
}

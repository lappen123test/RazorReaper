using RazorReaper.Models;

namespace RazorReaper.Services;

public interface INotificationService
{
    event Action<NotificationMessage>? OnNotificationAdded;
    event Action<string>? OnNotificationRemoved;

    void ShowSuccess(string message, int durationMs = 5000);
    void ShowError(string message, int durationMs = 7000);
    void ShowWarning(string message, int durationMs = 6000);
    void ShowInfo(string message, int durationMs = 5000);
    void RemoveNotification(string id);
}

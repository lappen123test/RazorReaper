using RazorReaper.Models;

namespace RazorReaper.Services;

/// <summary>
/// Service for managing application activity tracking.
/// Replaces static shared state with a thread-safe, injectable service.
/// </summary>
public interface IActivityService
{
    /// <summary>
    /// Adds a new activity to the recent activities list.
    /// </summary>
    /// <param name="title">The activity title/description.</param>
    /// <param name="type">The activity type (e.g., "info", "warning", "success").</param>
    void AddActivity(string title, string type = "info");

    /// <summary>
    /// Gets all recent activities, newest first.
    /// </summary>
    /// <returns>A list of recent ActivityItem objects.</returns>
    IReadOnlyList<ActivityItem> GetRecentActivities();

    /// <summary>
    /// Clears all activities.
    /// </summary>
    void ClearActivities();

    /// <summary>
    /// Event raised when a new activity is added.
    /// </summary>
    event EventHandler<ActivityItem>? ActivityAdded;
}

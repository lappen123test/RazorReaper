namespace RazorReaper.Models;

/// <summary>
/// Represents an activity item displayed in the recent activities feed.
/// </summary>
public class ActivityItem
{
    /// <summary>
    /// Gets or sets the title/description of the activity.
    /// </summary>
    public string Title { get; set; } = "";

    /// <summary>
    /// Gets or sets the timestamp when the activity occurred.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the type/category of the activity (e.g., "info", "warning", "success").
    /// </summary>
    public string Type { get; set; } = "info";

    /// <summary>
    /// Gets a human-readable representation of how long ago the activity occurred.
    /// </summary>
    /// <returns>A string like "Just now", "5m ago", "2h ago", "3d ago", or a date like "Jan 15".</returns>
    public string GetTimeAgo()
    {
        var span = DateTime.Now - Timestamp;

        if (span.TotalSeconds < 60)
            return "Just now";
        if (span.TotalMinutes < 60)
            return $"{(int)span.TotalMinutes}m ago";
        if (span.TotalHours < 24)
            return $"{(int)span.TotalHours}h ago";
        if (span.TotalDays < 7)
            return $"{(int)span.TotalDays}d ago";

        return Timestamp.ToString("MMM dd");
    }
}

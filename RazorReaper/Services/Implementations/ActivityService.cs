using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RazorReaper.Configuration;
using RazorReaper.Models;
using System.Collections.Concurrent;

namespace RazorReaper.Services.Implementations;

/// <summary>
/// Thread-safe implementation of IActivityService for managing application activities.
/// </summary>
public class ActivityService : IActivityService
{
    private readonly ILogger<ActivityService> _logger;
    private readonly AppConfiguration _config;
    private readonly ConcurrentBag<ActivityItem> _activities = new();
    private readonly object _lock = new();

    /// <inheritdoc/>
    public event EventHandler<ActivityItem>? ActivityAdded;

    public ActivityService(
        ILogger<ActivityService> logger,
        IOptions<AppConfiguration> config)
    {
        _logger = logger;
        _config = config.Value;
    }

    /// <inheritdoc/>
    public void AddActivity(string title, string type = "info")
    {
        try
        {
            var activity = new ActivityItem
            {
                Title = title,
                Type = type,
                Timestamp = DateTime.Now
            };

            _activities.Add(activity);
            _logger.LogDebug("Activity added: {Title} (Type: {Type})", title, type);

            // Trim old activities if we exceed the max count
            TrimActivities();

            // Raise event
            ActivityAdded?.Invoke(this, activity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding activity: {Title}", title);
        }
    }

    /// <inheritdoc/>
    public IReadOnlyList<ActivityItem> GetRecentActivities()
    {
        try
        {
            return _activities
                .OrderByDescending(a => a.Timestamp)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting recent activities");
            return new List<ActivityItem>();
        }
    }

    /// <inheritdoc/>
    public void ClearActivities()
    {
        try
        {
            lock (_lock)
            {
                _activities.Clear();
                _logger.LogInformation("All activities cleared");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing activities");
        }
    }

    private void TrimActivities()
    {
        try
        {
            var maxActivities = _config.Monitoring.MaxRecentActivities;
            if (_activities.Count > maxActivities)
            {
                lock (_lock)
                {
                    // Get the oldest activities to remove
                    var toRemove = _activities
                        .OrderBy(a => a.Timestamp)
                        .Take(_activities.Count - maxActivities)
                        .ToList();

                    // Note: ConcurrentBag doesn't have a Remove method, so we recreate it
                    var toKeep = _activities
                        .OrderByDescending(a => a.Timestamp)
                        .Take(maxActivities)
                        .ToList();

                    _activities.Clear();
                    foreach (var item in toKeep)
                    {
                        _activities.Add(item);
                    }

                    _logger.LogDebug("Trimmed {Count} old activities", toRemove.Count);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error trimming activities");
        }
    }
}

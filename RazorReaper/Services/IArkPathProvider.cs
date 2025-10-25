namespace RazorReaper.Services;

/// <summary>
/// Service for detecting and managing ARK: Survival Evolved installation paths.
/// </summary>
public interface IArkPathProvider
{
    /// <summary>
    /// Finds the ARK installation directory by scanning Steam registry and library folders.
    /// </summary>
    /// <returns>The full path to the ARK installation directory, or null if not found.</returns>
    string? FindArkPath();

    /// <summary>
    /// Gets the full path to the BaseDeviceProfiles.ini configuration file.
    /// </summary>
    /// <returns>The full path to the BaseDeviceProfiles.ini file, or null if ARK is not found.</returns>
    string? GetBaseDeviceProfilesPath();

    /// <summary>
    /// Validates whether a path points to a valid ARK installation.
    /// </summary>
    /// <param name="path">The path to validate.</param>
    /// <returns>True if the path contains a valid ARK installation; otherwise, false.</returns>
    bool IsValidArkPath(string path);
}

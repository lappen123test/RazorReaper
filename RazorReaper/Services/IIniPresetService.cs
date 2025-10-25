using RazorReaper.Models;

namespace RazorReaper.Services;

/// <summary>
/// Service for managing ARK INI configuration presets.
/// </summary>
public interface IIniPresetService
{
    /// <summary>
    /// Gets all available INI presets.
    /// </summary>
    /// <returns>A list of all available INI presets.</returns>
    List<IniPreset> GetAllPresets();

    /// <summary>
    /// Gets a specific preset by name.
    /// </summary>
    /// <param name="name">The preset name.</param>
    /// <returns>The matching preset, or null if not found.</returns>
    IniPreset? GetPresetByName(string name);

    /// <summary>
    /// Gets the image path for a preset.
    /// </summary>
    /// <param name="presetName">The preset name.</param>
    /// <returns>The relative path to the preset's image.</returns>
    string GetPresetImagePath(string presetName);
}

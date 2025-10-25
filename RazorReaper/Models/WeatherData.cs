namespace RazorReaper.Models;

/// <summary>
/// Represents weather information for a specific location.
/// </summary>
public class WeatherData
{
    /// <summary>
    /// Gets or sets the current temperature in Celsius.
    /// </summary>
    public int Temperature { get; set; }

    /// <summary>
    /// Gets or sets the "feels like" temperature in Celsius.
    /// </summary>
    public int FeelsLike { get; set; }

    /// <summary>
    /// Gets or sets the weather condition description (e.g., "Clear", "Cloudy", "Rainy").
    /// </summary>
    public string Condition { get; set; } = "";

    /// <summary>
    /// Gets or sets the city name.
    /// </summary>
    public string City { get; set; } = "";

    /// <summary>
    /// Gets or sets the country code.
    /// </summary>
    public string Country { get; set; } = "";
}

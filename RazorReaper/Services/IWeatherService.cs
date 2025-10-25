using RazorReaper.Models;

namespace RazorReaper.Services;

/// <summary>
/// Service for retrieving weather information.
/// </summary>
public interface IWeatherService
{
    /// <summary>
    /// Gets current weather data for the configured default city.
    /// Automatically tries fallback APIs if the primary API fails.
    /// </summary>
    /// <returns>WeatherData object containing current weather information, or null if all APIs fail.</returns>
    Task<WeatherData?> GetWeatherAsync();

    /// <summary>
    /// Gets current weather data for a specific city.
    /// </summary>
    /// <param name="city">The city name to get weather for.</param>
    /// <returns>WeatherData object containing current weather information, or null if all APIs fail.</returns>
    Task<WeatherData?> GetWeatherAsync(string city);
}

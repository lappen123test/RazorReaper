using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RazorReaper.Configuration;
using RazorReaper.Models;
using System.Text.Json;

namespace RazorReaper.Services.Implementations;

/// <summary>
/// Implementation of IWeatherService for retrieving weather information.
/// </summary>
public class WeatherService : IWeatherService
{
    private readonly ILogger<WeatherService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AppConfiguration _config;

    public WeatherService(
        ILogger<WeatherService> logger,
        IHttpClientFactory httpClientFactory,
        IOptions<AppConfiguration> config)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _config = config.Value;
    }

    /// <inheritdoc/>
    public async Task<WeatherData?> GetWeatherAsync()
    {
        return await GetWeatherAsync(_config.Weather.DefaultCity);
    }

    /// <inheritdoc/>
    public async Task<WeatherData?> GetWeatherAsync(string city)
    {
        try
        {
            _logger.LogInformation("Fetching weather data for city: {City}", city);

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(15);

            // Try primary API (ipapi.co + open-meteo)
            try
            {
                var weatherData = await FetchWeatherWithPrimaryApi(httpClient);
                if (weatherData != null)
                {
                    _logger.LogInformation("Successfully fetched weather data using primary API");
                    return weatherData;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "Primary weather API failed with HTTP error, trying fallback");
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "Failed to parse primary weather API response, trying fallback");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Primary weather API failed, trying fallback");
            }

            // Try fallback API (ipinfo.io + open-meteo)
            try
            {
                var weatherData = await FetchWeatherWithFallbackApi(httpClient);
                if (weatherData != null)
                {
                    _logger.LogInformation("Successfully fetched weather data using fallback API");
                    return weatherData;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Fallback weather API failed with HTTP error");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse fallback weather API response");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fallback weather API failed");
            }

            _logger.LogError("All weather APIs failed");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetWeatherAsync");
            return null;
        }
    }

    private async Task<WeatherData?> FetchWeatherWithPrimaryApi(HttpClient httpClient)
    {
        _logger.LogDebug("Fetching location from ipapi.co");
        var ipResponse = await httpClient.GetStringAsync("https://ipapi.co/json/");
        var ipData = JsonDocument.Parse(ipResponse);

        var lat = ipData.RootElement.GetProperty("latitude").GetDouble();
        var lon = ipData.RootElement.GetProperty("longitude").GetDouble();
        var city = ipData.RootElement.GetProperty("city").GetString() ?? "Unknown";
        var country = ipData.RootElement.GetProperty("country_name").GetString() ?? "Unknown";

        _logger.LogDebug("Location: {City}, {Country} ({Lat}, {Lon})", city, country, lat, lon);

        return await FetchWeatherFromOpenMeteo(httpClient, lat, lon, city, country);
    }

    private async Task<WeatherData?> FetchWeatherWithFallbackApi(HttpClient httpClient)
    {
        _logger.LogDebug("Fetching location from ipinfo.io (fallback)");
        var fallbackResponse = await httpClient.GetStringAsync("https://ipinfo.io/json");
        var fallbackData = JsonDocument.Parse(fallbackResponse);
        var loc = fallbackData.RootElement.GetProperty("loc").GetString()?.Split(',');

        if (loc == null || loc.Length != 2)
        {
            _logger.LogWarning("Invalid location data from fallback API");
            return null;
        }

        var lat = double.Parse(loc[0]);
        var lon = double.Parse(loc[1]);
        var city = fallbackData.RootElement.GetProperty("city").GetString() ?? "Unknown";
        var country = fallbackData.RootElement.GetProperty("country").GetString() ?? "Unknown";

        _logger.LogDebug("Fallback location: {City}, {Country} ({Lat}, {Lon})", city, country, lat, lon);

        return await FetchWeatherFromOpenMeteo(httpClient, lat, lon, city, country);
    }

    private async Task<WeatherData?> FetchWeatherFromOpenMeteo(
        HttpClient httpClient,
        double lat,
        double lon,
        string city,
        string country)
    {
        _logger.LogDebug("Fetching weather from Open-Meteo for coordinates: {Lat}, {Lon}", lat, lon);

        var weatherUrl = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current=temperature_2m,apparent_temperature,weather_code&temperature_unit=celsius";
        var weatherResponse = await httpClient.GetStringAsync(weatherUrl);
        var weatherData = JsonDocument.Parse(weatherResponse);

        var temp = weatherData.RootElement.GetProperty("current").GetProperty("temperature_2m").GetDouble();
        var feelsLike = weatherData.RootElement.GetProperty("current").GetProperty("apparent_temperature").GetDouble();
        var weatherCode = weatherData.RootElement.GetProperty("current").GetProperty("weather_code").GetInt32();

        var condition = GetWeatherCondition(weatherCode);

        _logger.LogDebug("Weather: {Temp}°C, {Condition}", temp, condition);

        return new WeatherData
        {
            Temperature = (int)Math.Round(temp),
            FeelsLike = (int)Math.Round(feelsLike),
            Condition = condition,
            City = city,
            Country = country
        };
    }

    private string GetWeatherCondition(int code)
    {
        return code switch
        {
            0 => "Clear",
            1 or 2 or 3 => "Partly Cloudy",
            45 or 48 => "Foggy",
            51 or 53 or 55 => "Drizzle",
            61 or 63 or 65 => "Rain",
            71 or 73 or 75 => "Snow",
            80 or 81 or 82 => "Showers",
            95 or 96 or 99 => "Thunderstorm",
            _ => "Unknown"
        };
    }
}

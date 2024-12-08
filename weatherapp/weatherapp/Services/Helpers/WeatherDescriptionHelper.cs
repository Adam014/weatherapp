using WeatherApp.Models;

namespace weatherapp.Services.Helpers
{
    public static class WeatherDescriptionHelper
    {
        // getting the weather description 
        public static string GetWeatherDescription(WeatherData weather)
        {
            if (weather.Weather != null && weather.Weather.Count > 0)
            {
                var main = weather.Weather[0]?.Main ?? "N/A";
                var description = weather.Weather[0]?.Description ?? "N/A";
                return $"{description} ({main})";
            }
            return "N/A";
        }

        // getting the visibility in kilometers
        public static string GetVisibilityInKm(int? visibility)
        {
            return visibility > 0 ? $"{visibility / 1000.0:F1} km" : "N/A";
        }

        // getting the wind description
        public static string GetWindDescription(WeatherData weather)
        {
            var speed = weather.Wind?.Speed ?? 0;
            var direction = weather.Wind?.Deg ?? 0;
            return $"{speed} m/s, Direction: {direction}°";
        }

        // getting the rain description
        public static string GetRainDescription(Rain rain)
        {
            return rain != null ? $"{rain.OneHour:F1} mm" : "No rain";
        }
    }
}

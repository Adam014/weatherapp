using System.Collections.Generic;
using WeatherApp.Models;

namespace WeatherApp.Helpers
{
    public static class WeatherFallbackProvider
    {
        // fallback WeatherData object with placeholder values
        public static WeatherData CreateFallbackWeatherData(string city, string errorDescription = "Unknown")
        {
            return new WeatherData
            {
                Name = city,
                Weather = new List<Weather>
                {
                    new Weather
                    {
                        Main = errorDescription,
                        Description = errorDescription,
                        Icon = "N/A"
                    }
                },
                Main = new Main
                {
                    Temp = 0.0,
                    Feels_Like = 0.0,
                    Temp_Min = 0.0,
                    Temp_Max = 0.0,
                    Pressure = 0,
                    Humidity = 0
                },
                Wind = new Wind
                {
                    Speed = 0.0,
                    Deg = 0
                },
                Clouds = new Clouds
                {
                    All = 0
                },
                Sys = new Sys
                {
                    Country = "N/A",
                    Sunrise = 0,
                    Sunset = 0
                },
                Visibility = 0
            };
        }
    }
}

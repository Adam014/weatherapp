using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.Helpers;

namespace WeatherApp.Services
{
    public class WeatherService
    {
        private readonly string _apiKey;
        private readonly string _apiHost;

        public WeatherService()
        {
            try
            {
                // ensure env file works
                if (!File.Exists("appsettings.json"))
                {
                    throw new FileNotFoundException("Configuration file 'appsettings.json' not found.");
                }

                // init api config
                var config = JsonSerializer.Deserialize<dynamic>(File.ReadAllText("appsettings.json"));
                _apiKey = config["RapidApiKey"];
                _apiHost = config["RapidApiHost"];

                // validate api envs
                if (string.IsNullOrEmpty(_apiKey) || string.IsNullOrEmpty(_apiHost))
                {
                    throw new InvalidOperationException("API key or host is not configured properly in 'appsettings.json'.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing WeatherService: {ex.Message}");
                throw; 
            }
        }

        // fetches weather data for a given city and language
        // returns WeatherData object with the fetched or fallback data
        public async Task<WeatherData> GetWeatherAsync(string city, string lang)
        {
            // validating input
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException("City name cannot be null or empty.", nameof(city));
            }

            try
            {
                // init http client
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("x-rapidapi-key", _apiKey);
                client.DefaultRequestHeaders.Add("x-rapidapi-host", _apiHost);

                var url = $"https://open-weather13.p.rapidapi.com/city/{city}/{lang}";
                var response = await client.GetFromJsonAsync<WeatherData>(url);

                if (response == null)
                {
                    Console.WriteLine("API returned null response.");
                    return WeatherFallbackProvider.CreateFallbackWeatherData(city);
                }

                return response;
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP error occurred: {httpEx.Message}");
                return WeatherFallbackProvider.CreateFallbackWeatherData(city, "Error fetching data");
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"Error parsing JSON response: {jsonEx.Message}");
                return WeatherFallbackProvider.CreateFallbackWeatherData(city, "Error parsing data");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return WeatherFallbackProvider.CreateFallbackWeatherData(city, "Unexpected error");
            }
        }
    }
}

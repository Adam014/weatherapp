using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class ErrorResponse
    {
        public string? Cod { get; set; }
        public string? Message { get; set; }
    }

    public class WeatherService
    {
        private readonly string _apiKey;
        private readonly string _apiHost;
        private readonly DatabaseService _databaseService;

        public WeatherService(DatabaseService databaseService)
        {
            _apiKey = Environment.GetEnvironmentVariable("RAPID_API_KEY") ?? throw new InvalidOperationException("API Key is missing.");
            _apiHost = Environment.GetEnvironmentVariable("RAPID_API_HOST") ?? throw new InvalidOperationException("API Host is missing.");
            _databaseService = databaseService;
        }

        public async Task<WeatherData?> GetWeatherAsync(string city, string lang)
        {
            try
            {
                // Check cached data
                var cachedData = _databaseService.GetWeatherData(city);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    return JsonSerializer.Deserialize<WeatherData>(cachedData);
                }

                // Fetch from API
                var response = await FetchWeatherFromApi(city, lang);
                if (response == null) return null;

                // Save to database
                var jsonData = JsonSerializer.Serialize(response);
                _databaseService.SaveWeatherData(city, jsonData);

                return response;
            }
            catch (Exception ex)
            {
                ShowMessageBox("Error", ex.Message, MessageBoxIcon.Error);
                return null;
            }
        }

        private async Task<WeatherData?> FetchWeatherFromApi(string city, string lang)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-rapidapi-key", _apiKey);
            client.DefaultRequestHeaders.Add("x-rapidapi-host", _apiHost);

            var url = $"https://open-weather13.p.rapidapi.com/city/{city}/{lang}";
            var httpResponse = await client.GetAsync(url);

            // We need to add check if the response is 200

            return await httpResponse.Content.ReadFromJsonAsync<WeatherData>();
        }

        public double ConvertTemperature(double temp, string unit) =>
            unit switch
            {
                "Celsius" => (temp - 32) * 5 / 9,
                "Fahrenheit" => temp,
                "Kelvin" => (temp - 32) * 5 / 9 + 273.15,
                _ => temp
            };

        private void ShowMessageBox(string title, string message, MessageBoxIcon icon)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }
    }
}

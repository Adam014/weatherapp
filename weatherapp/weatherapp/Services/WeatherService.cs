using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeatherApp.Helpers;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    // defined error resposne from the api endpoint
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

        // function for getting the weather, we check if the data needs to be updated, and when it does, we freshly fetch it from the api and save it to the db
        public async Task<WeatherData?> GetWeatherAsync(string city, string lang)
        {
            try
            {
                // Define data validity duration (e.g., 1 hour)
                var validityDuration = TimeSpan.FromHours(1);

                // check cached data
                if (!_databaseService.IsDataOutdated(city, validityDuration))
                {
                    var cachedData = _databaseService.GetWeatherData(city);
                    if (!string.IsNullOrEmpty(cachedData))
                    {
                        return JsonSerializer.Deserialize<WeatherData>(cachedData);
                    }
                }

                // fetch fresh data from API
                var response = await FetchWeatherFromApi(city, lang);
                if (response == null) return null;

                // save to database
                var jsonData = JsonSerializer.Serialize(response);
                _databaseService.SaveWeatherData(city, jsonData);

                return response;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage("Error", ex.Message, MessageBoxIcon.Error);
                return null;
            }
        }

        // function to fetch from the api
        private async Task<WeatherData?> FetchWeatherFromApi(string city, string lang)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-rapidapi-key", _apiKey);
            client.DefaultRequestHeaders.Add("x-rapidapi-host", _apiHost);

            var url = $"https://open-weather13.p.rapidapi.com/city/{city}/{lang}";
            var httpResponse = await client.GetAsync(url);

            // TODO: We need to add check if the response is 200

            return await httpResponse.Content.ReadFromJsonAsync<WeatherData>();
        }

        // function to convert temperatues (celsius, fahrenheit and kelvins)
        public double ConvertTemperature(double temp, string unit) =>
            unit switch
            {
                "Celsius" => (temp - 32) * 5 / 9,
                "Fahrenheit" => temp,
                "Kelvin" => (temp - 32) * 5 / 9 + 273.15,
                _ => temp
            };
    }
}

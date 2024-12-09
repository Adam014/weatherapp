using System.Net.Http.Json;
using System.Text.Json;
using WeatherApp.Helpers;
using WeatherApp.Models;

namespace WeatherApp.Services
{

    public class WeatherService(DatabaseService databaseService)
    {
        private readonly string _apiKey = Environment.GetEnvironmentVariable("RAPID_API_KEY") ?? throw new InvalidOperationException("API Key is missing.");
        private readonly string _apiHost = Environment.GetEnvironmentVariable("RAPID_API_HOST") ?? throw new InvalidOperationException("API Host is missing.");
        private readonly DatabaseService _databaseService = databaseService;

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
                        var weatherData = JsonSerializer.Deserialize<WeatherData>(cachedData);

                        // validate cached data
                        if (weatherData?.Weather != null && weatherData.Weather.Count > 0)
                        {
                            return weatherData;
                        }
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

        // get all the saved cities from the db
        public List<string> GetSavedCities()
        {
            return _databaseService.GetSavedCities();
        }

        // get last updated time for city
        public DateTime? GetLastUpdatedTime(string city)
        {
            return _databaseService.GetLastUpdatedTime(city);
        }

        // deleting city from db
        public void DeleteCityFromDatabase(string city)
        {
            _databaseService.DeleteCity(city);
        }

        // checking if provided city isnt already in database
        public bool IsCityInDatabase(string city)
        {
            return _databaseService.DoesCityExist(city);
        }

        // saving city to database
        public void SaveCityToDatabase(string city, string country)
        {
            _databaseService.SaveWeatherData(city, "{}");
        }

        // getting weather data for provided city
        public async Task<(WeatherData? WeatherData, string? ErrorMessage)> FetchWeatherForCity(string city, string lang)
        {
            try
            {
                var weatherData = await GetWeatherAsync(city, lang);
                if (weatherData == null || weatherData.Weather == null || weatherData.Weather.Count == 0)
                {
                    return (null, "Weather data is unavailable for the selected city.");
                }

                return (weatherData, null); // No error
            }
            catch (Exception ex)
            {
                return (null, $"Failed to fetch weather data. Error: {ex.Message}");
            }
        }
    }
}
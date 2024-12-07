using WeatherApp.Services;
using System;
using System.Windows.Forms;

namespace weatherapp
{
    public partial class Form1 : Form
    {
        private readonly WeatherService _weatherService;

        public Form1()
        {
            InitializeComponent();
            _weatherService = new WeatherService();
        }

        private async void FetchWeatherButton_Click(object sender, EventArgs e)
        {
            // Clear previous output and errors
            weatherOutput.Text = "";
            errorLabel.Visible = false;

            var city = cityInput.Text.Trim();

            if (string.IsNullOrEmpty(city))
            {
                errorLabel.Text = "Please enter a city.";
                errorLabel.Visible = true;
                return;
            }

            try
            {
                var weather = await _weatherService.GetWeatherAsync(city, "EN");

                // Display weather details
                weatherOutput.Text = $@"
City: {weather.Name}, {weather.Sys.Country}
Weather: {weather.Weather[0].Description} ({weather.Weather[0].Main})
Temperature: {weather.Main.Temp}°C
Feels Like: {weather.Main.Feels_Like}°C
Humidity: {weather.Main.Humidity}%
Wind: {weather.Wind.Speed} m/s, {weather.Wind.Deg}°
Cloudiness: {weather.Clouds.All}%
Rain (last hour): {weather.Rain?.OneHour ?? 0} mm
Sunrise: {UnixTimeStampToDateTime(weather.Sys.Sunrise)}
Sunset: {UnixTimeStampToDateTime(weather.Sys.Sunset)}";
            }
            catch (Exception ex)
            {
                errorLabel.Text = $"Error: {ex.Message}";
                errorLabel.Visible = true;
            }
        }

        private static string UnixTimeStampToDateTime(long unixTimeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).ToLocalTime().ToString("g");
        }
    }
}

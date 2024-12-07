using System;
using System.Windows.Forms;
using WeatherApp.Models;
using WeatherApp.Services;

namespace weatherapp
{
    public partial class Form1 : Form
    {
        private readonly WeatherService _weatherService;
        private string _currentUnit = "Celsius"; // Default temp unit

        public Form1()
        {
            InitializeComponent();
            var databaseService = new DatabaseService();
            _weatherService = new WeatherService(databaseService);
        }

        private async void FetchWeatherButton_Click(object sender, EventArgs e)
        {
            weatherGrid.Controls.Clear();

            var city = cityInput.Text.Trim();
            if (string.IsNullOrEmpty(city))
            {
                MessageBox.Show("Please enter a city.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var weather = await _weatherService.GetWeatherAsync(city, "CZ");
            if (weather != null)
            {
                DisplayWeatherData(weather);
                await SetAppIconAsync(weather.Weather[0].Icon);
            }
        }

        private void DisplayWeatherData(WeatherData weather)
        {
            AddCardToGrid("City", $"{weather.Name}, {weather.Sys.Country}");
            AddCardToGrid("Weather", $"{weather.Weather[0].Description} ({weather.Weather[0].Main})");

            var temp = _weatherService.ConvertTemperature(weather.Main.Temp, _currentUnit);
            var feelsLike = _weatherService.ConvertTemperature(weather.Main.Feels_Like, _currentUnit);
            AddCardToGrid("Temperature", $"{temp:F1}° {_currentUnit} (Feels like: {feelsLike:F1}° {_currentUnit})");

            var tempMin = _weatherService.ConvertTemperature(weather.Main.Temp_Min, _currentUnit);
            var tempMax = _weatherService.ConvertTemperature(weather.Main.Temp_Max, _currentUnit);
            AddCardToGrid("Min/Max Temp", $"Min: {tempMin:F1}° {_currentUnit}, Max: {tempMax:F1}° {_currentUnit}");

            AddCardToGrid("Humidity", $"{weather.Main.Humidity}%");
            AddCardToGrid("Wind", $"{weather.Wind.Speed} m/s, Direction: {weather.Wind.Deg}°");
            AddCardToGrid("Cloudiness", $"{weather.Clouds.All}%");
            AddCardToGrid("Sunrise", UnixTimeStampToDateTime(weather.Sys.Sunrise));
            AddCardToGrid("Sunset", UnixTimeStampToDateTime(weather.Sys.Sunset));
        }

        private void AddCardToGrid(string title, string value)
        {
            var card = new Panel
            {
                Size = new Size(300, 100),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.LightGray,
                Padding = new Padding(10),
                Margin = new Padding(10)
            };

            var titleLabel = new Label
            {
                Text = title,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter
            };

            var valueLabel = new Label
            {
                Text = value,
                Font = new Font("Arial", 12),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            card.Controls.Add(valueLabel);
            card.Controls.Add(titleLabel);

            weatherGrid.Controls.Add(card);
        }

        private async Task SetAppIconAsync(string iconId)
        {
            try
            {
                using var client = new HttpClient();
                var iconUrl = $"https://openweather.site/img/wn/{iconId}.png";
                var iconStream = await client.GetStreamAsync(iconUrl);

                using var bitmap = new Bitmap(iconStream);
                this.Icon = Icon.FromHandle(bitmap.GetHicon());
            }
            catch
            {
                // Fail silently for icon issues
            }
        }

        private static string UnixTimeStampToDateTime(long unixTimeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).ToLocalTime().ToString("g");
        }

        private void UnitToggle_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentUnit = unitToggle.SelectedItem.ToString();
            FetchWeatherButton_Click(sender, e);
        }
    }
}

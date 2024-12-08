using System.Drawing;
using System.Windows.Forms;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.Helpers
{
    public static class WeatherDataDisplayHelper
    {
        // displaying the cards into the grid
        public static void DisplayWeatherData(TableLayoutPanel weatherGrid, WeatherData weather, string unit, WeatherService service)
        {
            if (weather == null)
            {
                MessageHelper.ShowMessage("No weather data found. Please try again.", "Error", MessageBoxIcon.Warning);
                return;
            }

            try
            {
                weatherGrid.SuspendLayout();
                weatherGrid.Controls.Clear();

                var alternatingColors = new[] { Color.FromArgb(42, 42, 45), Color.FromArgb(26, 26, 29) };

                // Create and populate weather data dictionary
                var weatherData = GetWeatherDataDictionary(weather, unit, service);

                // Loop through the weather data and add cards
                int index = 0;
                foreach (var data in weatherData)
                {
                    AddOrUpdateWeatherCard(weatherGrid, data.Key, data.Value, alternatingColors[index % 2]);
                    index++;
                }

                weatherGrid.ResumeLayout();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage($"An error occurred: {ex.Message}", "Error", MessageBoxIcon.Error);
            }
        }

        private static Dictionary<string, string> GetWeatherDataDictionary(WeatherData weather, string unit, WeatherService service)
        {
            return new Dictionary<string, string>
            {
                { "City", $"{weather.Name ?? "N/A"}, {weather.Sys?.Country ?? "N/A"}" },
                { "Weather", GetWeatherDescription(weather) },
                { "Temperature", $"{service.ConvertTemperature(weather.Main?.Temp ?? 0, unit):F1}° {unit}" },
                { "Feels Like", $"{service.ConvertTemperature(weather.Main?.Feels_Like ?? 0, unit):F1}° {unit}" },
                { "Min Temp", $"{service.ConvertTemperature(weather.Main?.Temp_Min ?? 0, unit):F1}° {unit}" },
                { "Max Temp", $"{service.ConvertTemperature(weather.Main?.Temp_Max ?? 0, unit):F1}° {unit}" },
                { "Pressure", $"{(weather.Main?.Pressure ?? 0)} hPa" },
                { "Humidity", $"{(weather.Main?.Humidity ?? 0)}%" },
                { "Visibility", $"{GetVisibilityInKm(weather.Visibility)}" },
                { "Wind", $"{GetWindDescription(weather)}" },
                { "Cloudiness", $"{(weather.Clouds?.All ?? 0)}%" },
                { "Rain (Last 1h)", GetRainDescription(weather.Rain) },
                { "Sunrise", GetReadableDateTime(weather.Sys?.Sunrise) },
                { "Sunset", GetReadableDateTime(weather.Sys?.Sunset) }
            };
        }

        private static string GetWeatherDescription(WeatherData weather)
        {
            if (weather.Weather != null && weather.Weather.Count > 0)
            {
                var main = weather.Weather[0]?.Main ?? "N/A";
                var description = weather.Weather[0]?.Description ?? "N/A";
                return $"{description} ({main})";
            }
            return "N/A";
        }

        private static string GetVisibilityInKm(int? visibility)
        {
            return visibility > 0 ? $"{visibility / 1000.0:F1} km" : "N/A";
        }

        private static string GetWindDescription(WeatherData weather)
        {
            var speed = weather.Wind?.Speed ?? 0;
            var direction = weather.Wind?.Deg ?? 0;
            return $"{speed} m/s, Direction: {direction}°";
        }

        private static string GetRainDescription(Rain rain)
        {
            return rain != null ? $"{rain.OneHour:F1} mm" : "No rain";
        }

        private static string GetReadableDateTime(long? timestamp)
        {
            if (timestamp != null && timestamp > 0)
            {
                return UnixTimeStampToDateTime((long)timestamp);
            }
            return "N/A";
        }

        // updating only the temp fields when the value in the combo box changes
        public static void UpdateTemperatureFields(TableLayoutPanel weatherGrid, WeatherData weather, string unit, WeatherService service)
        {
            foreach (Control control in weatherGrid.Controls)
            {
                if (control is Panel panel && panel.Controls.Count > 1)
                {
                    var titleLabel = panel.Controls[1] as Label;
                    var valueLabel = panel.Controls[0] as Label;

                    if (titleLabel?.Text == "Temperature")
                        valueLabel.Text = $"{service.ConvertTemperature(weather.Main.Temp, unit):F1}° {unit}";
                    else if (titleLabel?.Text == "Feels Like")
                        valueLabel.Text = $"{service.ConvertTemperature(weather.Main.Feels_Like, unit):F1}° {unit}";
                    else if (titleLabel?.Text == "Min Temp")
                        valueLabel.Text = $"{service.ConvertTemperature(weather.Main.Temp_Min, unit):F1}° {unit}";
                    else if (titleLabel?.Text == "Max Temp")
                        valueLabel.Text = $"{service.ConvertTemperature(weather.Main.Temp_Max, unit):F1}° {unit}";
                }
            }
        }

        // adding or updating single card
        private static void AddOrUpdateWeatherCard(TableLayoutPanel weatherGrid, string title, string value, Color backgroundColor)
        {
            var container = new Panel
            {
                Padding = new Padding(10),
                BackColor = backgroundColor,
                Dock = DockStyle.Fill,
                Margin = new Padding(10)
            };

            container.Controls.Add(new Label
            {
                Text = value,
                Font = new Font("Arial", 12),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.White
            });

            container.Controls.Add(new Label
            {
                Text = title,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.White
            });

            int rowIndex = weatherGrid.RowCount;
            weatherGrid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            weatherGrid.RowCount++;
            weatherGrid.Controls.Add(container, rowIndex % 2, rowIndex / 2);
        }

        // converting unix timestamp to readable datetime
        private static string UnixTimeStampToDateTime(long unixTimeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).ToLocalTime().ToString("g");
        }
    }
}

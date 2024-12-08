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
            weatherGrid.SuspendLayout();

            weatherGrid.Controls.Clear();
            var alternatingColors = new[] { Color.FromArgb(42, 42, 45), Color.FromArgb(26, 26, 29) };

            AddOrUpdateWeatherCard(weatherGrid, "City", $"{weather.Name}, {weather.Sys.Country}", alternatingColors[0]);
            AddOrUpdateWeatherCard(weatherGrid, "Weather", $"{weather.Weather[0].Description} ({weather.Weather[0].Main})", alternatingColors[1]);
            AddOrUpdateWeatherCard(weatherGrid, "Temperature", $"{service.ConvertTemperature(weather.Main.Temp, unit):F1}° {unit}", alternatingColors[0]);
            AddOrUpdateWeatherCard(weatherGrid, "Feels Like", $"{service.ConvertTemperature(weather.Main.Feels_Like, unit):F1}° {unit}", alternatingColors[1]);
            AddOrUpdateWeatherCard(weatherGrid, "Min Temp", $"{service.ConvertTemperature(weather.Main.Temp_Min, unit):F1}° {unit}", alternatingColors[0]);
            AddOrUpdateWeatherCard(weatherGrid, "Max Temp", $"{service.ConvertTemperature(weather.Main.Temp_Max, unit):F1}° {unit}", alternatingColors[1]);
            AddOrUpdateWeatherCard(weatherGrid, "Pressure", $"{weather.Main.Pressure} hPa", alternatingColors[0]);
            AddOrUpdateWeatherCard(weatherGrid, "Humidity", $"{weather.Main.Humidity}%", alternatingColors[1]);
            AddOrUpdateWeatherCard(weatherGrid, "Visibility", $"{weather.Visibility / 1000.0:F1} km", alternatingColors[0]);
            AddOrUpdateWeatherCard(weatherGrid, "Wind", $"{weather.Wind.Speed} m/s, Direction: {weather.Wind.Deg}°", alternatingColors[1]);
            AddOrUpdateWeatherCard(weatherGrid, "Cloudiness", $"{weather.Clouds.All}%", alternatingColors[0]);
            AddOrUpdateWeatherCard(weatherGrid, "Rain (Last 1h)", weather.Rain != null ? $"{weather.Rain.OneHour:F1} mm" : "No rain", alternatingColors[1]);
            AddOrUpdateWeatherCard(weatherGrid, "Sunrise", UnixTimeStampToDateTime(weather.Sys.Sunrise), alternatingColors[0]);
            AddOrUpdateWeatherCard(weatherGrid, "Sunset", UnixTimeStampToDateTime(weather.Sys.Sunset), alternatingColors[1]);

            weatherGrid.ResumeLayout();
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

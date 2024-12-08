using System.Drawing;
using System.Windows.Forms;
using weatherapp.Services.Helpers;
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

                // create the dictionary
                var weatherData = GetWeatherDataDictionary(weather, unit, service);

                // loop throught the dictionary and create the cards
                int index = 0;
                foreach (var data in weatherData)
                {
                    WeatherCardHelper.AddOrUpdateWeatherCard(weatherGrid, data.Key, data.Value, alternatingColors[index % 2]);
                    index++;
                }

                weatherGrid.ResumeLayout();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage($"An error occurred: {ex.Message}", "Error", MessageBoxIcon.Error);
            }
        }

        // creating and returning instantly the dictionary with each weather data
        private static Dictionary<string, string> GetWeatherDataDictionary(WeatherData weather, string unit, WeatherService service)
        {
            return new Dictionary<string, string>
            {
                { "City", $"{weather.Name ?? "N/A"}, {weather.Sys?.Country ?? "N/A"}" },
                { "Weather", WeatherDescriptionHelper.GetWeatherDescription(weather) },
                { "Temperature", $"{service.ConvertTemperature(weather.Main?.Temp ?? 0, unit):F1}° {unit}" },
                { "Feels Like", $"{service.ConvertTemperature(weather.Main?.Feels_Like ?? 0, unit):F1}° {unit}" },
                { "Min Temp", $"{service.ConvertTemperature(weather.Main?.Temp_Min ?? 0, unit):F1}° {unit}" },
                { "Max Temp", $"{service.ConvertTemperature(weather.Main?.Temp_Max ?? 0, unit):F1}° {unit}" },
                { "Pressure", $"{(weather.Main?.Pressure ?? 0)} hPa" },
                { "Humidity", $"{(weather.Main?.Humidity ?? 0)}%" },
                { "Visibility", WeatherDescriptionHelper.GetVisibilityInKm(weather.Visibility) },
                { "Wind", WeatherDescriptionHelper.GetWindDescription(weather) },
                { "Cloudiness", $"{(weather.Clouds?.All ?? 0)}%" },
                { "Rain (Last 1h)", WeatherDescriptionHelper.GetRainDescription(weather.Rain) },
                { "Sunrise", DateTimeHelper.GetReadableDateTime(weather.Sys?.Sunrise) },
                { "Sunset", DateTimeHelper.GetReadableDateTime(weather.Sys?.Sunset) }
            };
        }

        // updating temp fields when we switch the temp unit in the combobox
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
    }
}

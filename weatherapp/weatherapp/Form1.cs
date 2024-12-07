using System;
using System.Windows.Forms;
using WeatherApp.Models;
using WeatherApp.Services;
using WeatherApp.Helpers;

namespace weatherapp
{
    public partial class Form1 : Form
    {
        private readonly WeatherService _weatherService;
        private WeatherData? _currentWeatherData;
        private string _currentUnit = "Celsius"; // Default temp unit

        public Form1()
        {
            InitializeComponent();
            // creating new weather service with the database service
            _weatherService = new WeatherService(new DatabaseService());
        }

        // function for the fetching data from api/db
        private async void FetchWeatherButton_Click(object sender, EventArgs e)
        {
            weatherGrid.Controls.Clear();

            string city = cityInput.Text.Trim();
            if (string.IsNullOrEmpty(city))
            {
                MessageHelper.ShowMessage("Please enter a city.", "Input Error", MessageBoxIcon.Warning);
                return;
            }

            _currentWeatherData = await _weatherService.GetWeatherAsync(city, "CZ");
            if (_currentWeatherData != null)
            {
                WeatherDataDisplayHelper.DisplayWeatherData(
                    weatherGrid,
                    _currentWeatherData,
                    _currentUnit,
                    _weatherService
                );
                await AppIconHelper.SetAppIconAsync(this, _currentWeatherData.Weather[0].Icon);
            }
        }

        // function to toggle between the temp values
        private void UnitToggle_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentUnit = unitToggle.SelectedItem.ToString();
            if (_currentWeatherData != null)
            {
                WeatherDataDisplayHelper.UpdateTemperatureFields(
                    weatherGrid,
                    _currentWeatherData,
                    _currentUnit,
                    _weatherService
                );
            }
        }

        // function to handle enter pressing 
        private void CityInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FetchWeatherButton_Click(sender, e);
                e.SuppressKeyPress = true; // Prevents the ding sound when pressing Enter
            }
        }
    }
}

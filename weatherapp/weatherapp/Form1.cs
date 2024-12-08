using System;
using System.Windows.Forms;
using WeatherApp.Models;
using WeatherApp.Services;
using WeatherApp.Helpers;
using weatherapp.Services.Helpers;

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
            _weatherService = new WeatherService(new DatabaseService());
        }

        // Fetch weather data when the button is clicked
        private async void FetchWeatherButton_Click(object sender, EventArgs e)
        {
            string city = cityInput.Text.Trim();

            if (string.IsNullOrEmpty(city))
            {
                MessageHelper.ShowMessage("Please enter a city.", "Input Error", MessageBoxIcon.Warning);
                return;
            }

            ResetWeatherGrid();

            SetLoadingState(true);

            _currentWeatherData = await _weatherService.GetWeatherAsync(city, "CZ");
            if (_currentWeatherData != null)
            {
                // Check if Weather data exists
                if (_currentWeatherData.Weather != null && _currentWeatherData.Weather.Count > 0)
                {
                    WeatherDataDisplayHelper.DisplayWeatherData(
                        weatherGrid,
                        _currentWeatherData,
                        _currentUnit,
                        _weatherService
                    );

                    await AppIconHelper.SetAppIconAsync(this, _currentWeatherData.Weather[0].Icon);
                }
                else
                {
                    MessageHelper.ShowMessage("Weather data is unavailable for the selected city.", "Error", MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageHelper.ShowMessage("Failed to fetch weather data. Please try again.", "Error", MessageBoxIcon.Warning);
            }

            SetLoadingState(false);
        }

        // function to reset the grid
        private void ResetWeatherGrid()
        {
            weatherGrid.SuspendLayout(); 
            weatherGrid.Controls.Clear();
            weatherGrid.RowStyles.Clear();
            weatherGrid.RowCount = 0;
            weatherGrid.ResumeLayout(); 
        }

        // Handle temperature unit change
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

        // Submit weather request on Enter key press
        private void CityInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FetchWeatherButton_Click(sender, e);
                e.SuppressKeyPress = true; // Prevents the ding sound when pressing Enter
            }
        }

        // Show or hide a loading indicator
        private void SetLoadingState(bool isLoading)
        {
            if (isLoading)
            {
                weatherGrid.Controls.Clear();
                var loadingLabel = new Label
                {
                    Text = "Loading...",
                    Font = new Font("Arial", 14, FontStyle.Bold),
                    ForeColor = Color.White,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                weatherGrid.Controls.Add(loadingLabel);
            }
        }
    }
}

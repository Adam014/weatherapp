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
        private bool isPanelVisible = true; 

        public Form1()
        {
            InitializeComponent();
            //var databaseService = new DatabaseService();
            //databaseService.ResetDatabase(); // TEMPORARY RESET
            _weatherService = new WeatherService(new DatabaseService());
            LoadCityList();
        }

        // fetching weather on the button click
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
                if (_currentWeatherData.Weather != null && _currentWeatherData.Weather.Count > 0)
                {
                    WeatherDataDisplayHelper.DisplayWeatherData(
                        weatherGrid,
                        _currentWeatherData,
                        _currentUnit,
                        _weatherService
                    );
                    await AppIconHelper.SetAppIconAsync(this, _currentWeatherData.Weather[0].Icon);

                    // Add city to the list
                    AddCityToList(city, _currentWeatherData.Sys?.Country ?? "N/A");
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

        // handeling change && converting temp with the selected temp unit 
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

        // submit request for the weather on enter keypress
        private void CityInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FetchWeatherButton_Click(sender, e);
                e.SuppressKeyPress = true; // Prevents the ding sound when pressing Enter
            }
        }

        // show or hide loader
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

        // load cities from the db into the listbox
        private void LoadCityList()
        {
            var cities = _weatherService.GetSavedCities();
            citiesListBox.Items.Clear();
            foreach (var city in cities)
            {
                if (!string.IsNullOrWhiteSpace(city)) 
                {
                    citiesListBox.Items.Add(city);
                }
            }
        }

        // add a city to the listbox
        private void AddCityToList(string city, string country)
        {
            string cityEntry = $"{city}, {country}";
            if (!citiesListBox.Items.Contains(cityEntry) && !string.IsNullOrWhiteSpace(city))
            {
                citiesListBox.Items.Add(cityEntry);
            }
        }

        // handle city selection from the ListBox
        private async void CitiesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (citiesListBox.SelectedItem is string selectedCity)
            {
                string city = selectedCity.Split(',')[0].Trim();
                ResetWeatherGrid();
                SetLoadingState(true);

                _currentWeatherData = await _weatherService.GetWeatherAsync(city, "CZ");
                if (_currentWeatherData != null)
                {
                    WeatherDataDisplayHelper.DisplayWeatherData(
                        weatherGrid,
                        _currentWeatherData,
                        _currentUnit,
                        _weatherService
                    );
                }

                SetLoadingState(false);
            }
        }

        // toggle function to hide or show the panel with the saved cities
        private void TogglePanelButton_Click(object sender, EventArgs e)
        {
            if (isPanelVisible)
            {
                // Hide the panel
                citiesPanel.Visible = false;
                togglePanelButton.Text = "<";
            }
            else
            {
                // Show the panel
                citiesPanel.Visible = true;
                togglePanelButton.Text = ">"; 
            }

            isPanelVisible = !isPanelVisible;
        }
    }
}

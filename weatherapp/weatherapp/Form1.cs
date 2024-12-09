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

            // check if the city already exists in the database
            if (_weatherService.IsCityInDatabase(city))
            {
                MessageBox.Show(
                    $"{city} is already saved in the database.",
                    "City Already Exists",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return; // exit early to avoid fetching and saving duplicate cities
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

                    UpdateLastUpdatedLabel(city);

                    // save city to database and UI
                    _weatherService.SaveCityToDatabase(city, _currentWeatherData.Sys?.Country ?? "N/A");
                    AddCityToFlowPanel(city, _currentWeatherData.Sys?.Country ?? "N/A");
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

        // load cities from the database into the FlowLayoutPanel
        private void LoadCityList()
        {
            var cities = _weatherService.GetSavedCities();
            var distinctCities = new HashSet<string>(cities);
            citiesFlowPanel.Controls.Clear();

            foreach (var city in distinctCities)
            {
                var parts = city.Split(',');
                if (parts.Length >= 2)
                {
                    AddCityToFlowPanel(parts[0].Trim(), parts[1].Trim());
                }
            }
        }

        // add each city to the flowpanel
        private void AddCityToFlowPanel(string city, string country)
        {
            string cityEntry = $"{city}, {country}";

            // City Panel
            var cityPanel = new Panel
            {
                Height = 40,
                Width = citiesFlowPanel.Width - 20,
                BackColor = Color.FromArgb(26, 26, 29),
                Padding = new Padding(5),
                Margin = new Padding(5)
            };

            // City Label
            var cityLabel = new Label
            {
                Text = cityEntry,
                ForeColor = Color.White,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            // Delete Button
            var deleteButton = new Button
            {
                Text = "Delete",
                BackColor = Color.FromArgb(166, 77, 121),
                ForeColor = Color.White,
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                Width = 60,
                Cursor = Cursors.Hand
            };

            deleteButton.FlatAppearance.BorderSize = 0;

            // deleting the city from the db & panel when clicking the delete button
            deleteButton.Click += (s, e) =>
            {
                _weatherService.DeleteCityFromDatabase(city);
                citiesFlowPanel.Controls.Remove(cityPanel);
            };

            // displaying the data for the city that we click on label or panel
            cityPanel.Click += async (s, e) => await DisplayWeatherForCity(city);
            cityLabel.Click += async (s, e) => await DisplayWeatherForCity(city);

            cityPanel.Controls.Add(deleteButton);
            cityPanel.Controls.Add(cityLabel);

            citiesFlowPanel.Controls.Add(cityPanel);
        }

        // display weather data for the city in the panel
        private async Task DisplayWeatherForCity(string city)
        {
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

                    UpdateLastUpdatedLabel(city);

                    // Ensure unit conversion logic is applied
                    UnitToggle_SelectedIndexChanged(null, null);
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

        // function to update lastupdated label
        private void UpdateLastUpdatedLabel(string city)
        {
            var lastUpdated = _weatherService.GetLastUpdatedTime(city);
            if (lastUpdated.HasValue)
            {
                lastUpdatedLabel.Text = $"Last Updated: {lastUpdated.Value:g} (CET)";
            }
            else
            {
                lastUpdatedLabel.Text = "Last Updated: N/A";
            }
        }
    }
}
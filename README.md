# WeatherApp 🌦️

 A sleek desktop weather application built with C# and WinForms. The app fetches weather data for cities, displays the data in an organized grid, and allows users to save favorite cities for quick access.

 ## Features 🎯
- Fetch Weather Data: Fetch real-time weather data for any city.
- Save Favorite Cities: Save cities for quick access.
- Temperature Units: Switch between Celsius, Fahrenheit, and Kelvin.
- Last Updated: View when the data was last updated.
- Delete Cities: Remove cities from your favorites list.
- Responsive UI: Elegant and intuitive design with a focus on usability.

## Prerequisites 📋
Before running the application, ensure you have:
1. .NET Framework: Version 4.7.2 or higher.
2. SQLite: Used as the local database.
3. API Key: Obtain a key from OpenWeather API and set the following environment variables.
    - RAPID_API_KEY: Your API key.
    - RAPID_API_HOST: Your API host.

## Getting Started 🚀

Clone the Repository
```bash
git clone https://github.com/yourusername/weatherapp.git
cd weatherapp
```
### Install Dependencies

No external libraries are required. Ensure you have a compatible .NET framework installed.

### Set Environment Variables
Create a .env file in the root of the project and the file will have this format:

```.env
RAPID_API_KEY={YOUR_API_KEY}
RAPID_API_HOST=open-weather13.p.rapidapi.com
```

### Run the Application
1. Open the solution (`weatherapp.sln`) in Visual Studio.
2. Build the solution.
3. Run the application.

## Application Usage 🖥️

### Main Features

  1. Fetch Weather:
      -  Enter a city name in the input field.
      -  Press Get Weather to fetch data.
  2. Save City:
      -  Cities are automatically saved to the database upon fetching weather data.
  3. Switch Temperature Units:
      -  Use the dropdown to switch between Celsius, Fahrenheit, and Kelvin.
  4. Manage Favorites:
      -  Click on a city in the side panel to view its weather data.
      -  Use the Delete button to remove a city from favorites.
  5. Last Updated:
      -  The timestamp of the last weather update is displayed at the top.

## Folder Structure 📂

```plaintext
weatherapp/
├── weatherapp/
│   ├── Models/
│   │   ├── WeatherData.cs
│   ├── Services/
│   │   ├── ── Helpers/
│   │   │      ├── AppIconHelper.cs
│   │   │      ├── DateTimeHelper.cs
│   │   │      ├── MessageHelper.cs
│   │   │      ├── WeatherCardHelper.cs
│   │   │      ├── WeatherDataDisplayHelper.cs
│   │   │      └── WeatherDescriptionHelper.cs
│   │   ├── DatabaseService.cs
│   │   └── WeatherService.cs
│   ├── Form1.cs
│   │   └── Form1.Designer.cs
```

## Documentation 📖
  1. Form1.cs:
  
      - Manages the main application UI and event handling.
      - Key methods:
          -  FetchWeatherButton_Click: Fetches weather data for a city.
          -  LoadCityList: Loads saved cities from the database.
          -  DisplayWeatherForCity: Displays weather data for a selected city.
  
  2. DatabaseService.cs:
  
      - Handles all database operations.
      - Key methods:
          -  SaveWeatherData: Inserts or updates weather data in the database.
          -  GetSavedCities: Retrieves a list of saved cities.
          -  eleteCity: Deletes a city from the database.
  
  3. WeatherService.cs:
  
      - Interacts with the weather API and manages data validation.
      - Key methods:
          -  GetWeatherAsync: Fetches weather data for a city.
          -  IsCityInDatabase: Checks if a city is already saved.
  
  4. WeatherDataDisplayHelper.cs:
  
      - Displays weather data in a grid format.
      - Key methods:
          - DisplayWeatherData: Populates the grid with weather data cards.
  
  5. WeatherCardHelper.cs:
  
      - Dynamically creates and updates UI elements.
      - Key method:
          -  AddCardToPanel: Adds a reusable card to a UI panel.



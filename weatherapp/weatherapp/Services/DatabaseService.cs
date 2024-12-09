using System.Data.SQLite;
using System.Text.Json;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;
        private readonly string _dbPath;

        public DatabaseService()
        {
            // ensure the database file is in the same directory as the executable
            _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "weather.db");
            _connectionString = $"Data Source={_dbPath};Version=3;";

            // init db
            InitializeDatabase();
        }

        // function to init of the db
        private void InitializeDatabase()
        {
            if (!File.Exists(_dbPath))
            {
                Console.WriteLine("Database file not found. Creating a new one...");
                SQLiteConnection.CreateFile(_dbPath);
            }

            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Weather (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    City TEXT NOT NULL,
                    Data TEXT NOT NULL,
                    LastUpdated DATETIME NOT NULL
                )";
            using var command = new SQLiteCommand(createTableQuery, connection);
            command.ExecuteNonQuery();
        }

        // func to get data from the db
        public string? GetWeatherData(string city)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string query = "SELECT Data FROM Weather WHERE City = @City";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@City", city);

            var result = command.ExecuteScalar();
            return result != null ? result.ToString() : null;
        }

        // func to save data to the db
        public void SaveWeatherData(string city, string data)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            // check if the city already exists
            string checkQuery = "SELECT COUNT(*) FROM Weather WHERE City = @City";
            using var checkCommand = new SQLiteCommand(checkQuery, connection);
            checkCommand.Parameters.AddWithValue("@City", city);

            var exists = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;

            // convert UTC time to CET
            var cetZone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
            var cetNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cetZone);

            if (exists)
            {
                // update existing record
                string updateQuery = "UPDATE Weather SET Data = @Data, LastUpdated = @LastUpdated WHERE City = @City";
                using var updateCommand = new SQLiteCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@City", city);
                updateCommand.Parameters.AddWithValue("@Data", data);
                updateCommand.Parameters.AddWithValue("@LastUpdated", cetNow);
                updateCommand.ExecuteNonQuery();
            }
            else
            {
                // insert new record
                string insertQuery = "INSERT INTO Weather (City, Data, LastUpdated) VALUES (@City, @Data, @LastUpdated)";
                using var insertCommand = new SQLiteCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@City", city);
                insertCommand.Parameters.AddWithValue("@Data", data);
                insertCommand.Parameters.AddWithValue("@LastUpdated", cetNow);
                insertCommand.ExecuteNonQuery();
            }
        }

        // func to check if the data in the rows are outdated
        public bool IsDataOutdated(string city, TimeSpan validityDuration)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string query = "SELECT LastUpdated FROM Weather WHERE City = @City";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@City", city);

            var result = command.ExecuteScalar();
            if (result != null && DateTime.TryParse(result.ToString(), out var lastUpdated))
            {
                return DateTime.UtcNow - lastUpdated > validityDuration;
            }

            // if no record is found or timestamp is invalid, consider the data outdated
            return true;
        }

        // get all cities that are saved in the db (format= city.name, city.country (Prague,CZ))
        public List<string> GetSavedCities()
        {
            var cities = new List<string>();

            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string query = "SELECT City FROM Weather";
            using var command = new SQLiteCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var cityName = reader["City"]?.ToString() ?? "Unknown";

                // add only the city to the list
                cities.Add(cityName);
            }

            return cities;
        }

        // debugging function to reset database when it is needed
        public void ResetDatabase()
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            // drop the Weather table if it exists
            string dropTableQuery = "DROP TABLE IF EXISTS Weather";
            using var dropCommand = new SQLiteCommand(dropTableQuery, connection);
            dropCommand.ExecuteNonQuery();

            // recreate the Weather table
            string createTableQuery = @"
        CREATE TABLE IF NOT EXISTS Weather (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            City TEXT NOT NULL,
            Data TEXT NOT NULL,
            LastUpdated DATETIME NOT NULL
        )";
            using var createCommand = new SQLiteCommand(createTableQuery, connection);
            createCommand.ExecuteNonQuery();

            Console.WriteLine("Database has been reset.");
        }

        // getting last updated time for city
        public DateTime? GetLastUpdatedTime(string city)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string query = "SELECT LastUpdated FROM Weather WHERE City = @City";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@City", city);

            var result = command.ExecuteScalar();
            if (result != null && DateTime.TryParse(result.ToString(), out var utcTime))
            {
                // Ensure the DateTime is treated as UTC
                return DateTime.SpecifyKind(utcTime, DateTimeKind.Utc);
            }

            return null;
        }

        // deleting city from the db
        public void DeleteCity(string city)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string query = "DELETE FROM Weather WHERE City = @City";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@City", city);

            command.ExecuteNonQuery();
        }

        // checking func if provided city is in db
        public bool DoesCityExist(string city)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string query = "SELECT COUNT(*) FROM Weather WHERE City = @City";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@City", city);

            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
    }
}
using System.Drawing;
using System.Windows.Forms;

namespace weatherapp.Services.Helpers
{
    public static class WeatherCardHelper
    {
        // adding or updating the card in the grid
        public static void AddOrUpdateWeatherCard(TableLayoutPanel weatherGrid, string title, string value, Color backgroundColor)
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
    }
}

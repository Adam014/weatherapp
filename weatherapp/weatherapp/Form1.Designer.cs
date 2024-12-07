namespace weatherapp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.cityInput = new System.Windows.Forms.TextBox();
            this.fetchWeatherButton = new System.Windows.Forms.Button();
            this.unitToggle = new System.Windows.Forms.ComboBox();
            this.weatherGrid = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();

            // City Input
            this.cityInput.Location = new System.Drawing.Point(50, 20);
            this.cityInput.Name = "cityInput";
            this.cityInput.PlaceholderText = "Enter city";
            this.cityInput.Size = new System.Drawing.Size(200, 27);
            this.cityInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CityInput_KeyDown);

            // Fetch Weather Button
            this.fetchWeatherButton.Location = new System.Drawing.Point(270, 20);
            this.fetchWeatherButton.Name = "fetchWeatherButton";
            this.fetchWeatherButton.Size = new System.Drawing.Size(100, 30);
            this.fetchWeatherButton.BackColor = Color.FromArgb(166, 77, 121);
            this.fetchWeatherButton.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            this.fetchWeatherButton.Text = "Get Weather";
            this.fetchWeatherButton.FlatStyle = FlatStyle.Flat;
            this.fetchWeatherButton.FlatAppearance.BorderSize = 0;
            this.fetchWeatherButton.ForeColor = Color.White;
            this.fetchWeatherButton.Click += new System.EventHandler(this.FetchWeatherButton_Click);

            // Unit Toggle
            this.unitToggle.Location = new System.Drawing.Point(400, 20);
            this.unitToggle.Name = "unitToggle";
            this.unitToggle.Size = new System.Drawing.Size(120, 27);
            this.unitToggle.Items.AddRange(new object[] { "Celsius", "Fahrenheit", "Kelvin" });
            this.unitToggle.SelectedIndex = 0;
            this.unitToggle.SelectedIndexChanged += new System.EventHandler(this.UnitToggle_SelectedIndexChanged);

            // Weather Grid (TableLayoutPanel)
            this.weatherGrid.Location = new System.Drawing.Point(50, 70);
            this.weatherGrid.Name = "weatherGrid";
            this.weatherGrid.AutoScroll = true;
            this.weatherGrid.Dock = DockStyle.Fill;
            this.weatherGrid.Padding = new Padding(10, 70, 10, 20); // Add padding at the bottom
            this.weatherGrid.Margin = new Padding(0, 0, 0, 70); // Ensure extra margin at the bottom
            this.weatherGrid.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.weatherGrid.ColumnCount = 2;
            this.weatherGrid.RowCount = 0;
            this.weatherGrid.ColumnStyles.Clear();
            this.weatherGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.weatherGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.weatherGrid.RowStyles.Clear();

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 900);
            this.BackColor = Color.FromArgb(26, 26, 29);
            this.Controls.Add(this.cityInput);
            this.Controls.Add(this.fetchWeatherButton);
            this.Controls.Add(this.unitToggle);
            this.Controls.Add(this.weatherGrid);
            this.Name = "Form1";
            this.Text = "Weather App";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox cityInput;
        private System.Windows.Forms.Button fetchWeatherButton;
        private System.Windows.Forms.ComboBox unitToggle;
        private System.Windows.Forms.TableLayoutPanel weatherGrid;

        #endregion
    }
}

﻿namespace weatherapp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel citiesPanel;
        private System.Windows.Forms.FlowLayoutPanel citiesFlowPanel;
        private System.Windows.Forms.Button togglePanelButton;
        private System.Windows.Forms.Label lastUpdatedLabel;

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
            this.citiesPanel = new System.Windows.Forms.Panel();
            this.citiesFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();

            // Cities Panel
            this.citiesPanel.Dock = DockStyle.Right;
            this.citiesPanel.Width = 200;
            this.citiesPanel.BackColor = Color.FromArgb(30, 30, 33);
            this.citiesPanel.Padding = new Padding(10);
            this.citiesPanel.Controls.Add(this.citiesFlowPanel); // Ensure citiesFlowPanel is added

            // Cities FlowLayoutPanel
            this.citiesFlowPanel.Dock = DockStyle.Fill;
            this.citiesFlowPanel.BackColor = Color.FromArgb(42, 42, 45);
            this.citiesFlowPanel.AutoScroll = true;
            this.citiesFlowPanel.FlowDirection = FlowDirection.TopDown;
            this.citiesFlowPanel.WrapContents = false;
            this.citiesFlowPanel.Padding = new Padding(5);

            // Toggle Panel Button
            this.togglePanelButton = new Button
            {
                Text = ">",
                Dock = DockStyle.Right,
                Width = 30,
                BackColor = Color.FromArgb(26, 26, 29),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            this.togglePanelButton.FlatAppearance.BorderSize = 0;
            this.togglePanelButton.Click += new EventHandler(this.TogglePanelButton_Click);

            // Control Panel (input & buttons)
            var controlsPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                Padding = new Padding(10),
                BackColor = Color.FromArgb(26, 26, 29)
            };
            controlsPanel.Controls.Add(cityInput);
            controlsPanel.Controls.Add(fetchWeatherButton);
            controlsPanel.Controls.Add(unitToggle);

            cityInput.Location = new System.Drawing.Point(10, 10);
            fetchWeatherButton.Location = new System.Drawing.Point(220, 10);
            unitToggle.Location = new System.Drawing.Point(350, 10);

            // City Input
            this.cityInput.Location = new System.Drawing.Point(50, 20);
            this.cityInput.Name = "cityInput";
            this.cityInput.PlaceholderText = "Enter city... (format: Prague)";
            this.cityInput.Size = new System.Drawing.Size(200, 27);
            this.cityInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CityInput_KeyDown);

            // Last Updated Label
            this.lastUpdatedLabel = new Label
            {
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.TopRight,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(26, 26, 29),
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                Padding = new Padding(10),
                Height = 40
            };
            this.lastUpdatedLabel.Text = "Last Updated: N/A";

            // Fetch Weather Button
            this.fetchWeatherButton.Location = new System.Drawing.Point(270, 20);
            this.fetchWeatherButton.Name = "fetchWeatherButton";
            this.fetchWeatherButton.Size = new System.Drawing.Size(100, 22);
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

            // Weather Grid
            weatherGrid.Dock = DockStyle.Fill;
            weatherGrid.AutoScroll = true;
            weatherGrid.Padding = new Padding(10);
            weatherGrid.Margin = new Padding(0);
            weatherGrid.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            weatherGrid.ColumnCount = 2;
            weatherGrid.ColumnStyles.Clear();
            weatherGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            weatherGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 930);
            this.BackColor = Color.FromArgb(26, 26, 29);
            this.Controls.Add(this.togglePanelButton);
            this.Controls.Add(this.citiesPanel);
            this.Controls.Add(weatherGrid);
            this.Controls.Add(controlsPanel);
            this.Controls.Add(this.lastUpdatedLabel);
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

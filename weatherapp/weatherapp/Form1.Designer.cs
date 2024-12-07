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
            this.errorLabel = new System.Windows.Forms.Label();
            this.weatherGrid = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();

            // City Input
            this.cityInput.Location = new System.Drawing.Point(50, 20);
            this.cityInput.Name = "cityInput";
            this.cityInput.PlaceholderText = "Enter city";
            this.cityInput.Size = new System.Drawing.Size(200, 27);

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
            this.weatherGrid.Size = new System.Drawing.Size(700, 400);
            this.weatherGrid.ColumnCount = 2;
            this.weatherGrid.RowCount = 1;
            this.weatherGrid.AutoScroll = true;
            this.weatherGrid.Dock = DockStyle.Fill;
            this.weatherGrid.Padding = new Padding(60);
            this.weatherGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.weatherGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.weatherGrid.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Error Label
            this.errorLabel.Location = new System.Drawing.Point(50, 550); // Adjust position to ensure it's visible
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(700, 30); // Large enough to display messages
            this.errorLabel.ForeColor = System.Drawing.Color.Red; // Use red color for error messages
            this.errorLabel.Font = new Font("Segoe UI", 10F); // Ensure it's readable
            this.errorLabel.TextAlign = ContentAlignment.MiddleLeft;
            this.errorLabel.Text = ""; // Ensure it's empty by default
            this.errorLabel.Visible = true; // Always visible

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 550);
            this.BackColor = Color.FromArgb(26, 26, 29);
            this.Controls.Add(this.cityInput);
            this.Controls.Add(this.fetchWeatherButton);
            this.Controls.Add(this.unitToggle);
            this.Controls.Add(this.weatherGrid);
            this.Controls.Add(this.errorLabel);
            this.Name = "Form1";
            this.Text = "Weather App";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox cityInput;
        private System.Windows.Forms.Button fetchWeatherButton;
        private System.Windows.Forms.ComboBox unitToggle;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.TableLayoutPanel weatherGrid;

        #endregion
    }
}

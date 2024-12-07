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
            this.weatherOutput = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();

            this.SuspendLayout();

            // City Input
            this.cityInput.Location = new System.Drawing.Point(50, 50);
            this.cityInput.Name = "cityInput";
            this.cityInput.PlaceholderText = "Enter city";
            this.cityInput.Size = new System.Drawing.Size(200, 27);

            // Fetch Weather Button
            this.fetchWeatherButton.Location = new System.Drawing.Point(270, 50);
            this.fetchWeatherButton.Name = "fetchWeatherButton";
            this.fetchWeatherButton.Size = new System.Drawing.Size(100, 30);
            this.fetchWeatherButton.Text = "Get Weather";
            this.fetchWeatherButton.Click += new System.EventHandler(this.FetchWeatherButton_Click);

            // Weather Output
            this.weatherOutput.Location = new System.Drawing.Point(50, 100);
            this.weatherOutput.Name = "weatherOutput";
            this.weatherOutput.Size = new System.Drawing.Size(400, 300);
            this.weatherOutput.Text = "";

            // Error Label
            this.errorLabel.Location = new System.Drawing.Point(50, 400);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(400, 30);
            this.errorLabel.ForeColor = System.Drawing.Color.Red;
            this.errorLabel.Text = "";
            this.errorLabel.Visible = false;

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cityInput);
            this.Controls.Add(this.fetchWeatherButton);
            this.Controls.Add(this.weatherOutput);
            this.Controls.Add(this.errorLabel);
            this.Name = "Form1";
            this.Text = "Weather App";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox cityInput;
        private System.Windows.Forms.Button fetchWeatherButton;
        private System.Windows.Forms.Label weatherOutput;
        private System.Windows.Forms.Label errorLabel;

        #endregion
    }
}

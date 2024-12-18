﻿using System.Windows.Forms;

namespace WeatherApp.Helpers
{
    public static class MessageHelper
    {
        // universal message box 
        public static void ShowMessage(string text, string title, MessageBoxIcon icon)
        {
            MessageBox.Show(text, title, MessageBoxButtons.OK, icon);
        }
    }
}

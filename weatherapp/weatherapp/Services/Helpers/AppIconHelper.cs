using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeatherApp.Helpers
{
    public static class AppIconHelper
    {
        public static async Task SetAppIconAsync(Form form, string iconId)
        {
            try
            {
                using var client = new HttpClient();
                var iconUrl = $"https://openweather.site/img/wn/{iconId}.png";
                var iconStream = await client.GetStreamAsync(iconUrl);

                using var bitmap = new Bitmap(iconStream);
                form.Icon = Icon.FromHandle(bitmap.GetHicon());
            }
            catch
            {
                // Fail silently for icon issues
            }
        }
    }
}

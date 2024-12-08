using System;

namespace weatherapp.Services.Helpers
{
    public static class DateTimeHelper
    {
        // getting the readable time
        public static string GetReadableDateTime(long? timestamp)
        {
            if (timestamp != null && timestamp > 0)
            {
                return UnixTimeStampToDateTime((long)timestamp);
            }
            return "N/A";
        }

        // formatting unix time to readable time
        private static string UnixTimeStampToDateTime(long unixTimeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).ToLocalTime().ToString("g");
        }
    }
}

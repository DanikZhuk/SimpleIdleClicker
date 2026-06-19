using System;

namespace Utils
{
    public static class StringFormatUtility
    {
        private const string Symbol = "$";
        private const string Time = "hour";
        
        public static string MoneyString(long money)
        {
            return $"{money:N0}{Symbol}";
        }
        
        public static string MoneySpeedString(long money)
        {
            return $"{money:N0}{Symbol}/{Time}";
        }
        
        public static string TimeString(float seconds)
        {
            var time = TimeSpan.FromSeconds(seconds);
            var format = "";
            if (time.Hours > 0)
            {
                format = @"h\:mm\:s";
            }
            else if (time.Minutes > 0)
            {
                format = @"m\:ss";
            }
            else if (time.Seconds > 0)
            {
                format = @"s\s";
            }
            else if (time.Milliseconds > 0)
            {
                format = @"s\s";
            }
            return time.ToString(format);
        }
    }
}
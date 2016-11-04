using System;

namespace UmbracoStarterKit.ExtensionMethods
{
    public static class DateExtentions
    {
        public static string ToFormattedDateTime(this DateTime date)
        {
            return date.ToString("%d") + GetDaySuffix(date.Day) + " " + date.ToString("MMMM yyyy");
        }

        private static string GetDaySuffix(int day)
        {
            switch (day)
            {
                case 1:
                case 21:
                case 31:
                    return "st";
                case 2:
                case 22:
                    return "nd";
                case 3:
                case 23:
                    return "rd";
                default:
                    return "th";
            }
        }
    }
}
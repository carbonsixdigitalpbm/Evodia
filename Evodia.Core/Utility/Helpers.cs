using System;
using System.Globalization;

namespace Evodia.Core.Utility
{
    public static class Helpers
    {
        public static int GetMonthNumber(string monthName)
        {
            try
            {
                return DateTime.ParseExact(monthName, "MMMM", CultureInfo.InvariantCulture).Month;
            }
            catch
            {
                return 0;
            }
        }

        public static string GetDaySuffix(string day)
        {
            switch (day)
            {
                case "1":
                case "21":
                case "31":
                    return "st";
                case "2":
                case "22":
                    return "nd";
                case "3":
                case "23":
                    return "rd";
                default:
                    return "th";
            }
        }
    }

}
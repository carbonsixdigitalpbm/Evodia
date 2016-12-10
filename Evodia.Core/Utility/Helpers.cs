using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Web;

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

        public static string MakeValidFileName(this string name)
        {
            var invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            var invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
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
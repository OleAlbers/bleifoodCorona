using Bleifood.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.BL
{


    public static class Helpers
    {
        private static Random _random = new Random();
        const string AllowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        public static readonly string[] ForbiddenUrls = { "manage", "edit", "mine", "truck", "admin", "help" };

        public static List<KeyValuePair<string, string>> Configuration { get; set; }

        public static GeoCoordinate ToDeviceLocation(this GeocodeSharp.Google.GeoCoordinate googleCoordinate)
        {
            return new GeoCoordinate(googleCoordinate.Latitude, googleCoordinate.Longitude);
        }

        public static string FromConfig(this string settingname)
        {
            return Configuration.FirstOrDefault(q => q.Key == settingname).Value;
        }

        public static DateTime FromHtmlTime(this object time)
        {
            var timeString = (string)time;
            var components = timeString.Split(":");
            if (components.Length < 2) return DateTime.Now;
            if (!int.TryParse(components[0], out int hour)) return DateTime.Now;
            if (!int.TryParse(components[1], out int minute)) return DateTime.Now;

            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute, 0);
        }

        public static string ToHtmlTime(this DateTime time)
        {
            return $"{time.Hour:00}:{time.Minute:00}:00";
        }

        public static double GetDistanceTo(this GeoCoordinate from, GeoCoordinate to)
        {
            var d1 = from.Latitude * (Math.PI / 180.0);
            var num1 = from.Longitude * (Math.PI / 180.0);
            var d2 = to.Latitude * (Math.PI / 180.0);
            var num2 = to.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))) / 1000;
        }

        public static int GetWeekOfYear(this DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday) time = time.AddDays(3);

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        // NOT safe for security-critical operations!
        public static string CreateRandomString(int length)
        {
            return new string(Enumerable.Repeat(AllowedCharacters, length).Select(q => q[_random.Next(q.Length)]).ToArray());
        }

        public static string AsCurrency(this decimal price)
        {
            return string.Format(new CultureInfo("de-de"), "{0:0.00} €", price);
        }

        public static Time NextQuarter(this Time time)
        {
            var retTime = new Time(time.Hour, time.Minute + 1);
            while (retTime.Minute % 15 != 0) retTime.Minute++;
            if (retTime.Minute >= 60)
            {
                retTime.Minute -= 60;
                retTime.Hour++;
            }
            return retTime;
        }
    }
}

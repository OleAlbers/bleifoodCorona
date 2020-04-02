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


        public static List<KeyValuePair<string, string>> Configuration { get; set; }

        public static GeoCoordinate ToDeviceLocation(this GeocodeSharp.Google.GeoCoordinate googleCoordinate)
        {
            return new GeoCoordinate(googleCoordinate.Latitude, googleCoordinate.Longitude);
        }



        public static string FromConfig(this string settingname)
        {
            return Configuration.FirstOrDefault(q => q.Key == settingname).Value;
            //return ConfigurationManager.AppSettings[settingname];
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
    }
}

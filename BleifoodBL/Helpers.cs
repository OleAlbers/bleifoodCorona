using System;
using System.Collections.Generic;
using System.Configuration;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.BL
{
    
    public static class Helpers
    {
        

        public static List<KeyValuePair<string,string>> Configuration { get; set; }

        public static GeoCoordinate ToDeviceLocation(this GeocodeSharp.Google.GeoCoordinate googleCoordinate)
        {
            return new GeoCoordinate(googleCoordinate.Latitude, googleCoordinate.Longitude);
        }

       

        public static string FromConfig(this string settingname)
        {
            return Configuration.FirstOrDefault(q=>q.Key==settingname).Value;
            //return ConfigurationManager.AppSettings[settingname];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaBL
{
    public static class Helpers
    {
        public static GeoCoordinate ToDeviceLocation(this GeocodeSharp.Google.GeoCoordinate googleCoordinate)
        {
            return new GeoCoordinate(googleCoordinate.Latitude, googleCoordinate.Longitude);
        }
    }
}

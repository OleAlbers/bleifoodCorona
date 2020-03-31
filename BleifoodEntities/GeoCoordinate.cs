using System;
using System.Collections.Generic;
using System.Text;

namespace Bleifood.Entities
{
    public class GeoCoordinate
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public GeoCoordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

       
    }
}

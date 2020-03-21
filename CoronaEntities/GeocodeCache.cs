using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaEntities
{
    // Geocode is EXPENSIVE
    public class GeocodeCache:BaseEntity
    {
        public string Address { get; set; } // Only store Hash 
        public GeoCoordinate Coordinate {get;set;}
    }
}

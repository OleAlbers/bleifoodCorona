//using GeocodeSharp.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using Google=GeocodeSharp.Google;

namespace Bleifood.BL
{

    // TODO: Check allowed Referers!
    public class Geocode
    {
        Google.GeocodeClient _gcClient;

        public Geocode(string api)
        {
            _gcClient = new Google.GeocodeClient(api);
        }

        private async Task<GeoCoordinate> GetCoordinates(string address)
        {
            var location = await _gcClient.GeocodeAddress(address, "de");
            if (location.Status != Google.GeocodeStatus.Ok) return null;
            return location.Results.First().Geometry.Location.ToDeviceLocation();
        }
        
        public double GetDistance(string truckAddress, string customerAddress)
        {
            var truckCoordinates = GetCoordinates(truckAddress).Result;
            if (truckCoordinates == null) return double.MaxValue;
            return GetDistance(truckCoordinates, customerAddress);
        }

        public double GetDistance(GeoCoordinate truckCoordinates, string customerAddress)
        {
            var customerCoordinates = GetCoordinates(customerAddress).Result;
            if (customerCoordinates == null || truckCoordinates == null) return double.MaxValue;

            return GetDistance(truckCoordinates, customerCoordinates);
        }

        public double GetDistance(GeoCoordinate truckLocation, GeoCoordinate customerLocation)
        {
            return truckLocation.GetDistanceTo(customerLocation);
        }
    }
}

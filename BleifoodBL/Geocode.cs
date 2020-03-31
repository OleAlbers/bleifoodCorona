//using GeocodeSharp.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google=GeocodeSharp.Google;
using Bleifood.DL.Interfaces;
using Bleifood.Entities;

namespace Bleifood.BL
{

    // TODO: Check allowed Referers
    public class Geocode
    {
        Google.GeocodeClient _gcClient;
        IGeoCode _geoCodeData = new DL.GeoCode();

        public Geocode(string api)
        {
            _gcClient = new Google.GeocodeClient(api);
        }

        public Geocode() {
            _gcClient = new Google.GeocodeClient(Config.Settings.GeocodeApi);
        }

        public async Task<GeoCoordinate> GetCoordinates(string address) // NEVER send this to anywhere in the frontend!
        {
            var storedLocation=_geoCodeData.GetForAddress(address);
            if (storedLocation != null) return new GeoCoordinate(storedLocation.Latitude, storedLocation.Longitude);
            
            var location = await _gcClient.GeocodeAddress(address, "de");
            if (location.Status != Google.GeocodeStatus.Ok) return null;
            var coordinates=location.Results.First().Geometry.Location.ToDeviceLocation();
            if (coordinates!=null)
            {
                _geoCodeData.StoreCode(new Entities.GeocodeCache
                {
                    Address = address,
                    Latitude=coordinates.Latitude,
                    Longitude=coordinates.Longitude
                });
            }
            return coordinates;
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

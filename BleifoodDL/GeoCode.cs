using Bleifood.DL.Interfaces;
using Bleifood.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bleifood.DL
{
    public class GeoCode : IGeoCode
    {
        public GeocodeCache GetForAddress(string address)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                var allCaches=connection.SelectAll<GeocodeCache>();
                return allCaches.FirstOrDefault(q => q.Address == address);
            }
        }

        public void StoreCode(GeocodeCache geocodeCache)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Insert(geocodeCache);
            }
        }
    }
}

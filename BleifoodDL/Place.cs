using Bleifood.DL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.DL
{
    public class Place : IPlace
    {
        

        public void Delete(Guid id)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Delete<Bleifood.Entities.Place>(id);

            }
        }

        public IEnumerable<Bleifood.Entities.Place> GetForFoodtruck(Guid id)
        {
            var allPlaces = SelectAll();
            return allPlaces.Where(q => q.TruckId == id);
        }

        public void Insert(Bleifood.Entities.Place place)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Insert(place);
            }
        }

        public IEnumerable<Bleifood.Entities.Place> SelectAll()
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                return connection.SelectAll<Bleifood.Entities.Place>();

            }
        }

        public void Update(Bleifood.Entities.Place place)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Update(place);
            }
        }
    }
}

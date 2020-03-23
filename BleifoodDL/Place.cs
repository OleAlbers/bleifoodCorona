using CoronaDL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaDL
{
    public class Place : IPlace
    {
        

        public void Delete(Guid id)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Delete<CoronaEntities.Place>(id);

            }
        }

        public IEnumerable<CoronaEntities.Place> GetForFoodtruck(Guid id)
        {
            var allPlaces = SelectAll();
            return allPlaces.Where(q => q.TruckId == id);
        }

        public void Insert(CoronaEntities.Place place)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Insert(place);
            }
        }

        public IEnumerable<CoronaEntities.Place> SelectAll()
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                return connection.SelectAll<CoronaEntities.Place>();

            }
        }

        public void Update(CoronaEntities.Place place)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Update(place);
            }
        }
    }
}

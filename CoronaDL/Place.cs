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
        private DataConnection _connection = new DataConnection();

        public void Delete(Guid id)
        {
            _connection.Delete<CoronaEntities.Place>(id);
        }

        public IEnumerable<CoronaEntities.Place> GetForFoodtruck(Guid id)
        {
            var allPlaces = SelectAll();
            return allPlaces.Where(q => q.TruckId == id);
        }

        public void Insert(CoronaEntities.Place place)
        {
            _connection.Insert(place);
        }

        public IEnumerable<CoronaEntities.Place> SelectAll()
        {
            return _connection.SelectAll<CoronaEntities.Place>();
        }

        public void Update(CoronaEntities.Place place)
        {
            _connection.Update(place);
        }
    }
}

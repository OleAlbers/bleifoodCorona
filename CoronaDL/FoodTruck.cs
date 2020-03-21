using CoronaDL.Interfaces;

using System.Collections.Generic;

namespace CoronaDL
{
    public class FoodTruck : IFoodTruck
    {
        private DataConnection _connection = new DataConnection();

        public void Insert(CoronaEntities.FoodTruck truck)
        {
            _connection.Insert(truck);
        }

        public IEnumerable<CoronaEntities.FoodTruck> SelectAll()
        {
            return _connection.SelectAll<CoronaEntities.FoodTruck>();
        }

        public void Update(CoronaEntities.FoodTruck truck)
        {
            _connection.Update(truck);
        }
    }
}
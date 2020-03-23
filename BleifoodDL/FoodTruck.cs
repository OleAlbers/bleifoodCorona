using CoronaDL.Interfaces;

using System.Collections.Generic;

namespace CoronaDL
{
    public class FoodTruck : IFoodTruck
    {
        public void Insert(CoronaEntities.FoodTruck truck)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Insert(truck);
            }
                
        }

        public IEnumerable<CoronaEntities.FoodTruck> SelectAll()
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                return connection.SelectAll<CoronaEntities.FoodTruck>();
            }
        }

        public void Update(CoronaEntities.FoodTruck truck)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Update(truck);
            }
        }
    }
}
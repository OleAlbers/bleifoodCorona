using Bleifood.DL.Interfaces;

using System.Collections.Generic;

namespace Bleifood.DL
{
    public class FoodTruck : IFoodTruck
    {
        public void Insert(Bleifood.Entities.FoodTruck truck)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Insert(truck);
            }
        }

        public IEnumerable<Bleifood.Entities.FoodTruck> SelectAll()
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                return connection.SelectAll<Bleifood.Entities.FoodTruck>();
            }
        }

        public void Update(Bleifood.Entities.FoodTruck truck)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Update(truck);
            }
        }
    }
}
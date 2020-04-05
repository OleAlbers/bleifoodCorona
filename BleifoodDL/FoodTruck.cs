using Bleifood.DL.Interfaces;
using System;
using System.Collections.Generic;

namespace Bleifood.DL
{
    public class FoodTruck : IFoodTruck
    {
        public Entities.FoodTruck GetByUrl(string url)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                var truckList=connection.GetCollection<Entities.FoodTruck>();
                return truckList.FindOne(q => q.Url == url);
            }
        }

        public Entities.FoodTruck GetById(Guid id)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                return connection.SelectById<Entities.FoodTruck>(id);
            }
        }

        public void Insert(Bleifood.Entities.FoodTruck truck)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Insert(truck);
            }
        }

        public IEnumerable<Entities.FoodTruck> SelectAll()
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                return connection.SelectAll<Bleifood.Entities.FoodTruck>();
            }
        }

        public void Update(Entities.FoodTruck truck)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Update(truck);
            }
        }
    }
}
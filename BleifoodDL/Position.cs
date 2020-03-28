using Bleifood.DL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.DL
{
    public class Position : IPosition
    {
        

        public void Delete(Guid id)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Delete<Bleifood.Entities.Position>(id);
            }
        }

        public void DeleteForTruck(Guid id)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                foreach (var position in GetForTruck(id))
                {
                    connection.Delete<Bleifood.Entities.Position>(position.Id);
                }
            }
        }

        public IEnumerable<Bleifood.Entities.Position> GetForTruck(Guid truckId)
        {
            return SelectAll().Where(q => q.TruckId == truckId);
        }

        public void Insert(Bleifood.Entities.Position position)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Insert(position);
            }
        }

        public  IEnumerable<Bleifood.Entities.Position> SelectAll()
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                return connection.SelectAll<Bleifood.Entities.Position>();
            }
        }

        public void Update(Bleifood.Entities.Position position)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Update(position);
            }
        }
    }
}

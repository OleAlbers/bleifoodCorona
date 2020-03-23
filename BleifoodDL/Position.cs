using CoronaDL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaDL
{
    public class Position : IPosition
    {
        

        public void Delete(Guid id)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Delete<CoronaEntities.Position>(id);
            }
        }

        public void DeleteForTruck(Guid id)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                foreach (var position in GetForTruck(id))
                {
                    connection.Delete<CoronaEntities.Position>(position.Id);
                }
            }
        }

        public IEnumerable<CoronaEntities.Position> GetForTruck(Guid truckId)
        {
            return SelectAll().Where(q => q.TruckId == truckId);
        }

        public void Insert(CoronaEntities.Position position)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Insert(position);
            }
        }

        public  IEnumerable<CoronaEntities.Position> SelectAll()
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                return connection.SelectAll<CoronaEntities.Position>();
            }
        }

        public void Update(CoronaEntities.Position position)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Update(position);
            }
        }
    }
}

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
        private DataConnection _connection = new DataConnection();

        public void Delete(Guid id)
        {
            _connection.Delete<CoronaEntities.Position>(id);
        }

        public void DeleteForTruck(Guid id)
        {
            foreach (var position in GetForTruck(id))
            {
                _connection.Delete<CoronaEntities.Position>(position.Id);
            }
        }

        public IEnumerable<CoronaEntities.Position> GetForTruck(Guid truckId)
        {
            return SelectAll().Where(q => q.TruckId == truckId);
        }

        public void Insert(CoronaEntities.Position position)
        {
            _connection.Insert(position);
        }

        public  IEnumerable<CoronaEntities.Position> SelectAll()
        {
            return _connection.SelectAll<CoronaEntities.Position>();
        }

        public void Update(CoronaEntities.Position position)
        {
            _connection.Update(position);
        }
    }
}

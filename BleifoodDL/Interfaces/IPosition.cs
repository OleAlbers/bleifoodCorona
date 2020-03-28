using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.DL.Interfaces
{
    public interface IPosition
    {
        void Insert(Bleifood.Entities.Position position);
        void Update(Bleifood.Entities.Position position);
        IEnumerable<Bleifood.Entities.Position> SelectAll();
        IEnumerable<Bleifood.Entities.Position> GetForTruck(Guid truckId);
        void DeleteForTruck(Guid id);
    }
}

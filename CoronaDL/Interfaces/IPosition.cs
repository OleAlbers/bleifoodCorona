using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaDL.Interfaces
{
    public interface IPosition
    {
        void Insert(CoronaEntities.Position position);
        void Update(CoronaEntities.Position position);
        IEnumerable<CoronaEntities.Position> SelectAll();
        void Delete(Guid id);
    }
}

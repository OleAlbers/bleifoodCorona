using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaDL
{
    public interface IPosition
    {
        void Insert(CoronaEntities.Position position);
        void Update(CoronaEntities.Position position);
        void SelectAll();
        void Delete(Guid id);
    }
}

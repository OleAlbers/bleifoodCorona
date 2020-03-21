using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaDL
{
    public interface IPlace
    {
        void Insert(CoronaEntities.Place place);
        void Update(CoronaEntities.Place place);
        IEnumerable<CoronaEntities.Place> GetForFoodtruck(Guid id);
        void SelectAll();
        void Delete(Guid id);
    }
}

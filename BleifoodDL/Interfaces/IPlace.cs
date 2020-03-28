using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.DL.Interfaces
{
    public interface IPlace
    {
        void Insert(Bleifood.Entities.Place place);
        void Update(Bleifood.Entities.Place place);
        IEnumerable<Bleifood.Entities.Place> GetForFoodtruck(Guid id);
        IEnumerable<Bleifood.Entities.Place> SelectAll();
        void Delete(Guid id);
    }
}

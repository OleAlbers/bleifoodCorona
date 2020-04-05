using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.DL.Interfaces
{
    public interface IFoodTruck
    {
        void Insert(Entities.FoodTruck truck);
        void Update(Entities.FoodTruck truck);
        IEnumerable<Entities.FoodTruck> SelectAll();
        Entities.FoodTruck GetByUrl(string url);
        Entities.FoodTruck GetById(Guid id);

    }
}

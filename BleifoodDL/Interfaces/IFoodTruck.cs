using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.DL.Interfaces
{
    public interface IFoodTruck
    {
        void Insert(CoronaEntities.FoodTruck truck);
        void Update(CoronaEntities.FoodTruck truck);
        IEnumerable<CoronaEntities.FoodTruck> SelectAll();
    }
}

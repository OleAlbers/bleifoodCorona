using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaDL
{
    public interface IFoodTruck
    {
        void Insert(CoronaEntities.FoodTruck truck);
        void Update(CoronaEntities.FoodTruck truck);
        void SelectAll();
    }
}

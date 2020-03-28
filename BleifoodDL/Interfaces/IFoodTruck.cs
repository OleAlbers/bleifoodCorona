using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.DL.Interfaces
{
    public interface IFoodTruck
    {
        void Insert(Bleifood.Entities.FoodTruck truck);
        void Update(Bleifood.Entities.FoodTruck truck);
        IEnumerable<Bleifood.Entities.FoodTruck> SelectAll();
    }
}

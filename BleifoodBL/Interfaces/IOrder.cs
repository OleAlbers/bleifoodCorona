using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaBL.Interfaces
{
    public interface IOrder
    {
        void PlaceOrder(CoronaEntities.Order order);
    }
}

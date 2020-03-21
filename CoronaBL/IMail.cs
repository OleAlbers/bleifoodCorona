using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaBL
{
    public interface IMail
    {
        void SendMail(CoronaEntities.User user, string title, string content);
        void Validate(CoronaEntities.User user);
        void OrderCustomer(CoronaEntities.Order order);
        void OrderFoodTruck(CoronaEntities.Order order);
    }
}

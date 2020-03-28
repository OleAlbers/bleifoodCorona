using AspNetCore.Identity.LiteDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.BL.Interfaces
{
    public interface IMail
    {
        void SendMail(ApplicationUser user, string subject, string body);
        void Validate(ApplicationUser user, string token);
        void OrderCustomer(Bleifood.Entities.Order order);
        void OrderFoodTruck(Bleifood.Entities.Order order);
    }
}

using AspNetCore.Identity.LiteDB.Models;

namespace Bleifood.BL.Interfaces
{
    public interface IMail
    {
        void SendMail(ApplicationUser user, string subject, string body);

        void Validate(ApplicationUser user, string token);

        void OrderCustomer(Entities.Order order, Entities.FoodTruck truck);

        void OrderFoodTruck(Entities.Order order, Entities.FoodTruck truck);
    }
}
using Bleifood.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bleifood.BL
{
    public class Order : IOrder
    {
        private IMail _mailLogic = new Mail();
        private IFoodTruck _truckLogic = new FoodTruck();
        public void PlaceOrder(Entities.Order order)
        {
            var truck = _truckLogic.GetTruck(order.TruckId);
            if (truck == null) throw new Exception("wrong truck");
            order.UniqueKey = Helpers.CreateRandomString(8);
            _mailLogic.OrderFoodTruck(order, truck);
            _mailLogic.OrderCustomer(order, truck);
        }
    }
}

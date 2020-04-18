using System.Linq;

namespace Bleifood.BL
{
    public static class OderHelpers
    {
        private const string Currency = "eur";

        public static decimal LinePrice(this Entities.OrderPosition position)
        {
            if (position.Amount < 0) return 0;
            return position.Amount * position.Position.Price;
        }

        public static decimal SumPositions(this Entities.Order order)
        {
            return order.Positions.Sum(q => q.Amount * q.Position.Price);
        }

        public static bool MinPriceReached(this Entities.Order order)
        {
            return order.SumPositions() >= order.Truck.MinimumOrderAmount;
        }

        public static decimal ShippingCost(this Entities.Order order)
        {
            decimal shipping = order.Truck.ShippingCost;
            if (order.SumPositions() >= order.Truck.MinimumNoShippingAmount && order.Truck.OfferNoShipping) shipping = 0;
            return shipping;
        }

        public static decimal Total(this Entities.Order order)
        {
            decimal sum = order.SumPositions() + order.ShippingCost() + order.Tip;
            return sum;
        }

        // https://www.paypal.me/stammtischphilosoph/33.01eur
        public static string PaypalMe(this Entities.Order order)
        {
            return $"{order.Truck.PaypalMe}/{order.Total()}{Currency}";
        }

        public static string PaypalMe(this Entities.FoodTruck truck, decimal total)
        {
            return $"{truck.PaypalMe}/{total}{Currency}";
        }
    }
}
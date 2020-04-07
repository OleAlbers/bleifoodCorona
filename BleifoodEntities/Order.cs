using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.Entities
{
    public class Order : BaseEntity
    {

        public decimal Tip { get; set; }
        public Address CustomerAddress { get; set; }
        public List<OrderPosition> Positions { get; set; } = new List<OrderPosition>();
        public Time TimeSlot { get; set; } = new Time();
        public string Comment { get; set; }
  
        public virtual FoodTruck Truck { get; set; }
        public string UniqueKey { get; set; }

        public Order (FoodTruck truck)
        {
            Truck = truck;
        }

        public string TippText
        {
            get
            {
                return string.Format(new CultureInfo("de-de"), "{0:0.00}", Tip);
            }
            set
            {
                if (decimal.TryParse(value, NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint, new CultureInfo("de-de"), out decimal result))
                {
                    Tip = result;
                };
                
            }
        }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaEntities
{
    public class Order : BaseEntity
    {
        public decimal Tipp { get; set; }
        public Customer Customer { get; set; }
        public IEnumerable<OrderPosition> Positions { get; set; }
        public DateTime TimeSlot { get; set; }
        public string Comment { get; set; }
        public decimal Shipping { get; set; }
        public FoodTruck Truck { get; set; }
        public string UniqueKey { get; set; }
        
        
    }
}

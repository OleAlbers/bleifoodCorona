using CoronaEntities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaEntities
{
    public class FoodTruck:BaseEntity
    {
       
        public string PaypalMail { get; set; }
        public Address PostAddress { get; set; }

        [ShowToCustomer]
        public string Url { get; set; }


        public bool Active { get; set; }

        public DateTime StartDelivery { get; set; }
        public DateTime EndDelivery { get; set; }
        public DateTime StartOrder { get; set; }
    }
}

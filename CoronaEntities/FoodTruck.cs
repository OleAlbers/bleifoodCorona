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
        [ShowToCustomer]
        public string ContactMail { get; set; }
   
        [ShowToCustomer]
        public string Address { get; set; }
        [ShowToCustomer]
        public string ContactPhone { get; set; }
        [ShowToCustomer]
        public string Name { get; set; }
        [ShowToCustomer]
        public string Url { get; set; }


        public bool Active { get; set; }
    }
}

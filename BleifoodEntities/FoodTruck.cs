using Bleifood.Entities.Attributes;
using Bleifood.Entities.Validators;
using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.Entities
{
    public class FoodTruck : BaseEntity
    {
        [Required]
        public string Name {get;set;}
        //public string PaypalMail { get; set; }
        [Required, PaypalMe]
        public string PaypalMe {get;set;}
        public Address PostAddress { get; set; }

        [ShowToCustomer]
        [Required,RegularExpression("[a..z][0..9]", ErrorMessage = "Es sind nur Buchstaben und Zahlen erlaubt")]
        public string Url { get; set; }


        public bool Active { get; set; }

        public DateTime StartDelivery { get; set; }
        public DateTime EndDelivery { get; set; }
        public DateTime StartOrder { get; set; }
        public bool TakeAway { get; set; }
        public string UserId { get; set; }
    }
}

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
        [Required,RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Es sind nur Buchstaben und Zahlen erlaubt")]
        public string Url { get; set; }


        public bool Active { get; set; } = false;
        [Required]
        public Time StartDelivery { get; set; }
        [Required]
        public Time EndDelivery { get; set; }
        [Required]
        public Time StartOrder { get; set; }
        public bool TakeAway { get; set; }
        public string UserId { get; set; }
        public bool InDataBase { get; set; } = false;
        public string InfoText { get; set; }
        public string TestToken { get; set; }
        public DateTime? TestFinished { get; set; }
        [Required]
        public decimal MinimumOrderAmount { get; set; }
        public string MinimumOrderAmountAsText
        {
            get
            {
                return MinimumOrderAmount.FromCurrency();
            }
            set
            {
                MinimumOrderAmount = value.ToCurrency();
            }
        }
        [Required]
        public decimal MinimumNoShippingAmount { get; set; }
        public string MinimumNoShippingAmountAsText
        {
            get
            {
                return MinimumNoShippingAmount.FromCurrency();
            } set
            {

                MinimumNoShippingAmount = value.ToCurrency();
            }
        }
        public bool OfferNoShipping { get; set; }
        [Required, Range(1,100)]
        public decimal ShippingCost { get; set; }
        public string ShippingCostAsText
        {
            get
            {
                return ShippingCost.FromCurrency();
            }
            set
            {

                ShippingCost = value.ToCurrency();
            }
        }
    }
}

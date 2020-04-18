using Bleifood.Entities.Validators;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.Entities
{
    public class Position : BaseEntity
    {
        public enum VeganTypes { NotVegan, Vegan, Vegetarian }
        public enum Categories { Dessert, SideOrder, Main, Drink, Soup, Salad }
        public Guid TruckId { get; set; }
        public int SortOrder { get; set; }
        [Required]
        public string Name { get; set; }
        public string Subtitle { get; set; }
        public decimal Price { get; set; }
        [Required, RegularExpression("\\d+(,\\d{1,2})?", ErrorMessage = "Bitte einen gültigen Geldbetrag eingeben")]
        public string PriceAsText
        {
            get
            {
                return Price.FromCurrency();
            }
            set
            {
                Price = value.ToCurrency();
            }
        }
        [Required]
        public Categories Category { get; set; }
        [Required]
        public VeganTypes Vegan { get; set; }
    }
}

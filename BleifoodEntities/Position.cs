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
        public string Name { get; set; }
        public string Subtitle { get; set; }
        public decimal Price { get; set; }
        [RegularExpression("\\d+(,\\d{1,2})?", ErrorMessage = "Bitte einen gültigen Geldbetrag eingeben")]
        public string PriceAsText
        {
            get
            {
                return Price.ToString(CultureInfo.GetCultureInfo("de-de"));
            }
            set
            {
                try
                {
                    Price = decimal.Parse(value, CultureInfo.GetCultureInfo("de-de"));
                }
                catch (Exception)
                {
                    Price = 0;
                }
            }
        }
        public Categories Category { get; set; }
        public VeganTypes Vegan { get; set; }

    }
}

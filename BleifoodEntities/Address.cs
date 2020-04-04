using Bleifood.Entities.Attributes;
using Bleifood.Entities.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.Entities
{
    public class Address
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string Zip { get; set; }
        [Required]
        public string City { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Street) && string.IsNullOrWhiteSpace(City)) return null;

            return ($"{Street}{(!string.IsNullOrWhiteSpace(Street) ? "," : "")} {Zip} {City}\n").Trim();
        }

        private void SplitCity(string zipcity)
        {
            var firstSpace = zipcity.IndexOf(' ');
            if (firstSpace > 0)
            {
                Zip = zipcity.Substring(0, firstSpace).Trim();
                City = zipcity.Substring(firstSpace).Trim();
            }
            else
            {
                City = zipcity;
            }
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Street) && !string.IsNullOrEmpty(City);
        }

        public string AsString
        {
            get
            {
                return this.ToString();
            }
            set
            {
                    Street = null;
                Zip = null;
                City = null;

                int commaPos = value.IndexOf(',');
                if (commaPos <1) return;
                Street = value.Substring(0, commaPos);
                SplitCity(value.Substring(commaPos + 1).Trim());
            }
        }
    }

}

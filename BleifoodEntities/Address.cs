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
            return $"{Street}\n{Zip}\n{City}\n";
        }
    }
}

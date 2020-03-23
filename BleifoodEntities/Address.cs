using CoronaEntities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaEntities
{
    public class Address
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public override string ToString()
        {
            return $"{Street}\n{Zip}\n{City}\n";
        }
    }
}

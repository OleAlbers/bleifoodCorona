using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaEntities
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }
        public string Road { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
    }
}

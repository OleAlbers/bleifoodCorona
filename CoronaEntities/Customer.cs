using CoronaEntities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaEntities
{
    public class Customer : BaseEntity
    {
        [StoreInCookie]
        public string Name { get; set; }
        [StoreInCookie]
        public string Road { get; set; }
        [StoreInCookie]
        public string City { get; set; }
        [StoreInCookie]
        public string Phone { get; set; }
        [StoreInCookie]
        public string Mail { get; set; }
    }
}

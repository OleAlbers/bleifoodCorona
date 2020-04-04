using Bleifood.Entities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.Entities
{
    public class Customer : BaseEntity
    {
        public Address PostAddress { get; set; } = new Address();
    }
}

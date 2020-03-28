using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.Entities
{
    public class OrderPosition:BaseEntity
    {
        public int Amount { get; set; }
        public Position Position { get; set; }
    }
}

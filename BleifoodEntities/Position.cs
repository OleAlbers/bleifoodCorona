using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.Entities
{
    public class Position:BaseEntity
    {
        public Guid TruckId { get; set; }
        public int SortOrder { get; set; }
        public string Name { get; set; }
        public string Subtitle { get; set; }
        public decimal Price { get; set; }

    }
}

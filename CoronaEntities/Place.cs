using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaEntities
{
    public class Place:BaseEntity
    {
        public Guid TruckId { get; set; }
        public  long CoordsH { get; set; }
        public long CoordsV { get; set; }
        public string Road { get; set; }
        public string City { get; set; }
        public double Distance { get; set; }
        public DateTime StartDelivery { get; set; }
        public DateTime EndDelivery { get; set; }
        public DateTime StartOrder { get; set; }
    }
}

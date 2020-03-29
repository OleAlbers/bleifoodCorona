using Bleifood.Entities.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.Entities
{
    public class Place:BaseEntity
    {
        public Guid TruckId { get; set; }
        public  long CoordsH { get; set; }
        public long CoordsV { get; set; }
        [Required]
        public string Road { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public double Distance { get; set; }
  
    }
}

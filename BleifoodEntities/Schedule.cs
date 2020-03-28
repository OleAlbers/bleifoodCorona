using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.Entities
{
    public class Schedule:BaseEntity
    {
        public bool IsEven { get; set; }
        public DayOfWeek Weekday { get; set; }
        public Guid? PlaceId { get; set; }
        public Guid TruckId { get; set; }
    }
}

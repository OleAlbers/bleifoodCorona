using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaEntities
{
    public class Schedule
    {
        public bool IsEven { get; set; }
        public int Weekday { get; set; }
        public Guid? PlaceId { get; set; }
        public Guid TruckId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaEntities
{
    public class Schedule
    {
        public enum Weekdays { Montag, Dienstag, Mittwoch, Donnerstag, Samstag,Sonntag };
        public bool IsEven { get; set; }
        public Weekdays Weekday { get; set; }
        public Guid PlaceId { get; set; }
    }
}

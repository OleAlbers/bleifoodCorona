using System;
using System.Collections.Generic;
using System.Text;

namespace Bleifood.Entities
{
    public class ScheduleWeek
    {
        public Guid? Monday { get; set; }
        public Guid? Tuesday { get; set; }
        public Guid? Wednesday { get; set; }
        public Guid? Thursday { get; set; }
        public Guid? Friday { get; set; }
        public Guid? Saturday { get; set; }
        public Guid? Sunday { get; set; }
    }
}

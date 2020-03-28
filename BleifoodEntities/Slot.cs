﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.Entities
{
    public class Slot:BaseEntity    
    {
        public Guid TruckId { get; set; }
        public Guid ScheduleId { get; set; }
        public DateTime SlotTime { get; set; }
        public bool IsOpen { get; set; }
    }
}

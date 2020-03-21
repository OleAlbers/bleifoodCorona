using CoronaDL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaDL
{
    public class Slot : ISlot
    {
        private DataConnection _connection = new DataConnection();

        public IEnumerable<CoronaEntities.Slot> GetForSchedule(Guid id)
        {
            var allSlots=_connection.SelectAll<CoronaEntities.Slot>();
            return allSlots.Where(q => q.ScheduleId == id);
        }

        public void UpdateForSchedule(IEnumerable<CoronaEntities.Slot> slots)
        {
            foreach (var slot in slots)
            {
                _connection.Update(slot);
            }
        }
    }
}

using CoronaDL.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;

namespace CoronaDL
{
    public class Slot : ISlot
    {
        private DataConnection _connection = new DataConnection();

        public void DeleteForSchedule(Guid id)
        {
            foreach (var slot in GetForSchedule(id))
            {
                _connection.Delete<CoronaEntities.Slot>(slot.Id);
            }
        }

        public IEnumerable<CoronaEntities.Slot> GetForSchedule(Guid id)
        {
            var allSlots = _connection.SelectAll<CoronaEntities.Slot>();
            return allSlots.Where(q => q.ScheduleId == id);
        }

        public void Insert(CoronaEntities.Slot slot)
        {
            throw new NotImplementedException();
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
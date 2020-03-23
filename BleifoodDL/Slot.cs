using CoronaDL.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;

namespace CoronaDL
{
    public class Slot : ISlot
    {
        

        public void DeleteForSchedule(Guid id)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                foreach (var slot in GetForSchedule(id))
                {
                    connection.Delete<CoronaEntities.Slot>(slot.Id);
                }
            }
        }

        public IEnumerable<CoronaEntities.Slot> GetForSchedule(Guid id)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                var allSlots = connection.SelectAll<CoronaEntities.Slot>();
                return allSlots.Where(q => q.ScheduleId == id);
            }
        }

        public void Insert(CoronaEntities.Slot slot)
        {
            throw new NotImplementedException();
        }

        public void UpdateForSchedule(IEnumerable<CoronaEntities.Slot> slots)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                foreach (var slot in slots)
                {
                    connection.Update(slot);
                }
            }
        }
    }
}
using Bleifood.DL.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Bleifood.DL
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
                    connection.Delete<Entities.Slot>(slot.Id);
                }
            }
        }

        public IEnumerable<Entities.Slot> GetForSchedule(Guid id)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                var allSlots = connection.SelectAll<Bleifood.Entities.Slot>();
                return allSlots.Where(q => q.ScheduleId == id);
            }
        }

        public void Insert(Entities.Slot slot)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Insert(slot);
            }
        }

        public void UpdateForSchedule(IEnumerable<Entities.Slot> slots)
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
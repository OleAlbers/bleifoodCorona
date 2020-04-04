using System;
using System.Collections.Generic;
using System.Text;

namespace Bleifood.BL.Interfaces
{
    public interface IToday
    {
        Entities.Schedule GetTodaysSchedule(Guid truckId);
        IEnumerable<Entities.Slot> GetTodaysSlots(Guid truckId);

        void IncreaseSlot(Guid slotId);
        void OpenSlot(Guid slotId);
        void CloseSlot(Guid slotId);
        void DeleteOldSlots();


    }
}

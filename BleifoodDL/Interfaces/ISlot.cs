using System;
using System.Collections.Generic;

namespace CoronaDL.Interfaces
{
    public interface ISlot
    {
        IEnumerable<CoronaEntities.Slot> GetForSchedule(Guid id);

        void UpdateForSchedule(IEnumerable<CoronaEntities.Slot> slots);
        void DeleteForSchedule(Guid id);
        void Insert(CoronaEntities.Slot slot);
    }
}
using System;
using System.Collections.Generic;

namespace Bleifood.DL.Interfaces
{
    public interface ISlot
    {
        IEnumerable<Bleifood.Entities.Slot> GetForSchedule(Guid id);

        void UpdateForSchedule(IEnumerable<Bleifood.Entities.Slot> slots);
        void DeleteForSchedule(Guid id);
        void Insert(Bleifood.Entities.Slot slot);
    }
}
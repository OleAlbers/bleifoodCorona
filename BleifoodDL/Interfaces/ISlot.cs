using System;
using System.Collections.Generic;

namespace Bleifood.DL.Interfaces
{
    public interface ISlot
    {
        IEnumerable<Bleifood.Entities.Slot> GetForSchedule(Guid id);

        void UpdateForSchedule(IEnumerable<Bleifood.Entities.Slot> slots);
        void Update(Entities.Slot slot);
        void Insert(Bleifood.Entities.Slot slot);
        Entities.Slot Get(Guid id);

        IEnumerable<Entities.Slot> GetAll();
        void Delete(Guid id);
    }
}
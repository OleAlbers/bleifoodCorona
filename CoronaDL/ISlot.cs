using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaDL
{
    public interface ISlot
    {
        IEnumerable<CoronaEntities.Slot> GetForSchedule(Guid id);
        void UpdateForSchedule(Guid id, IEnumerable<CoronaEntities.Slot> slots);
    }
}

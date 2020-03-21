using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaDL.Interfaces
{
    public interface ISchedule
    {
        IEnumerable<CoronaEntities.Schedule> GetForFoodtruck(Guid id);
        void UpdateForFoodtruck( IEnumerable<CoronaEntities.Schedule> schedule);
    }
}

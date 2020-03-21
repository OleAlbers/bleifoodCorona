using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaDL
{
    public interface ISchedule
    {
        IEnumerable<CoronaEntities.Schedule> GetForFoodtruck(Guid id);
        void UpdateForFoodtruck(Guid id, IEnumerable<CoronaEntities.Schedule> schedule);
    }
}

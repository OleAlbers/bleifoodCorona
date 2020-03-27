using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.DL.Interfaces
{
    public interface ISchedule
    {
        IEnumerable<CoronaEntities.Schedule> GetForFoodtruck(Guid id);
        void UpdateForFoodtruck(IEnumerable<CoronaEntities.Schedule> schedule);
        void DeleteOldSchedule(Guid truckId);
        void InsertSchedule(CoronaEntities.Schedule schedule);
        CoronaEntities.Schedule GetById(Guid id);
    }
}

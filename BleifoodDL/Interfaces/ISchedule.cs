using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.DL.Interfaces
{
    public interface ISchedule
    {
        IEnumerable<Bleifood.Entities.Schedule> GetForFoodtruck(Guid id);
        void UpdateForFoodtruck(IEnumerable<Bleifood.Entities.Schedule> schedule);
        void DeleteOldSchedule(Guid truckId);
        void InsertSchedule(Bleifood.Entities.Schedule schedule);
        Bleifood.Entities.Schedule GetById(Guid id);
        IEnumerable<Bleifood.Entities.Schedule> SelectAll();
    }
}

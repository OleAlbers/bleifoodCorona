using CoronaDL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaDL
{
    public class Schedule : ISchedule
    {
        private DataConnection _connection = new DataConnection();

        public IEnumerable<CoronaEntities.Schedule> GetForFoodtruck(Guid id)
        {
            var allSchedules = _connection.SelectAll<CoronaEntities.Schedule>();
            return allSchedules.Where(q => q.TruckId == id);
        }

        public void UpdateForFoodtruck(IEnumerable<CoronaEntities.Schedule> schedule)
        {
            foreach (var scheduleItem in schedule)
            {
                _connection.Update(scheduleItem);
            }
        }
    }
}

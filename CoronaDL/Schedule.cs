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
        public void DeleteOldSchedule(Guid truckId)
        {
            var oldSchedule = GetForFoodtruck(truckId);
            foreach (var oldScheduleItem in oldSchedule)
            {
                _connection.Delete<CoronaEntities.Schedule>(oldScheduleItem.Id);
            }
        }

        public void InsertSchedule(CoronaEntities.Schedule schedule)
        {
            _connection.Insert(schedule);
        }

        public CoronaEntities.Schedule GetById(Guid id)
        {
            return _connection.SelectById<CoronaEntities.Schedule>(id);
        }
    }
}

using Bleifood.DL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.DL
{
    public class Schedule : ISchedule
    {
        

        public IEnumerable<CoronaEntities.Schedule> GetForFoodtruck(Guid id)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                var allSchedules = connection.SelectAll<CoronaEntities.Schedule>();
                return allSchedules.Where(q => q.TruckId == id);
            }
        }

        public void UpdateForFoodtruck(IEnumerable<CoronaEntities.Schedule> schedule)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                foreach (var scheduleItem in schedule)
                {
                    connection.Update(scheduleItem);
                }
            }
        }
        public void DeleteOldSchedule(Guid truckId)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                var oldSchedule = GetForFoodtruck(truckId);
                foreach (var oldScheduleItem in oldSchedule)
                {
                    connection.Delete<CoronaEntities.Schedule>(oldScheduleItem.Id);
                }
            }
        }

        public void InsertSchedule(CoronaEntities.Schedule schedule)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Insert(schedule);

            }
        }

        public CoronaEntities.Schedule GetById(Guid id)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                return connection.SelectById<CoronaEntities.Schedule>(id);
            }
        }
    }
}

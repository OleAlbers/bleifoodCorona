using Bleifood.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bleifood.BL
{
    public class Today : IToday
    {
        private readonly IFoodTruck _truckLogic = new FoodTruck();
        private readonly DL.Interfaces.ISlot _dbSlot = new DL.Slot();

        private bool IsEvenWeek()
        {
            return DateTime.Now.GetWeekOfYear() % 2 == 0;
        }

        public void CloseSlot(Guid slotId)
        {
            var slot = _dbSlot.Get(slotId);
            slot.IsOpen = false;
            _dbSlot.Update(slot);
        }

        public void DeleteOldSlots()
        {
            foreach (var slot in _dbSlot.GetAll().Where(q=>q.Created<DateTime.Today))
            {
                _dbSlot.Delete(slot.Id);
            }
        }

        public Entities.Schedule GetTodaysSchedule(Guid truckId)
        {
            var scheduleForTruck = _truckLogic.GetSchedule(truckId);
            var scheduleForThisWeek = scheduleForTruck.Where(q => q.IsEven == IsEvenWeek());
            return scheduleForThisWeek.First(q => q.Weekday == DateTime.Now.DayOfWeek);
        }

        public IEnumerable<Entities.Slot> GetTodaysSlots(Guid truckId)
        {
            var truck = _truckLogic.GetTruck(truckId);
            var todaySchedule = GetTodaysSchedule(truckId);
            var todaysSlots = _truckLogic.GetSlots(todaySchedule.Id).ToList();
            if (todaysSlots.Count()==0)
            {
                Entities.Time slotDate = MakeQuarter(truck.StartDelivery);
                while (slotDate.CompareTo(truck.EndDelivery)<0)
                {
                    var slot=new Entities.Slot
                    {
                        ScheduleId = todaySchedule.Id,
                        SlotTime = slotDate,
                        TruckId = truckId
                    };
                    _dbSlot.Insert(slot);
                    slotDate.AddMinutes(15);
                }
                todaysSlots = _truckLogic.GetSlots(todaySchedule.Id).ToList();
            }
            return todaysSlots;
        }

        Entities.Time MakeQuarter(Entities.Time time)
        {
            int minute = 0;
            if (time.Minute >= 15) minute = 15;
            if (time.Minute >= 30) minute = 30;
            if (time.Minute >= 45) minute = 45;

            return new Entities.Time(time.Hour, minute);
        }

        public void IncreaseSlot(Guid slotId)
        {
            var slot = _dbSlot.Get(slotId);
            slot.OrderCount++;
            _dbSlot.Update(slot);
        }

        public void OpenSlot(Guid slotId)
        {
            var slot = _dbSlot.Get(slotId);
            slot.IsOpen=true;
            _dbSlot.Update(slot);
        }
    }
}

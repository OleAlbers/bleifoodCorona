using CoronaBL.Interfaces;

using CoronaEntities;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaBL
{
    public class Foodtruck : IFoodtruck
    {
        private CoronaDL.Interfaces.IFoodTruck _dbFoodTruck = new CoronaDL.FoodTruck();
        private IUser _user;
        private CoronaDL.Interfaces.IPlace _dbPlace = new CoronaDL.Place();
        private CoronaDL.Interfaces.IPosition _dbPosition = new CoronaDL.Position();
        private CoronaDL.Interfaces.ISchedule _dbSchedule = new CoronaDL.Schedule();
        private CoronaDL.Interfaces.ISlot _dbSlot = new CoronaDL.Slot();

        public Foodtruck(IUser user)
        {
            _user = user;
        }

        private bool CheckPermission(Guid? truckId)
        {
            return false;
        }

        public void CreateTruck(FoodTruck foodtruck)
        {
            CheckPermission(null);
            var existingTruck = GetAllTrucks().FirstOrDefault(q => q.Url.Equals(foodtruck.Url, StringComparison.InvariantCultureIgnoreCase));
            if (existingTruck != null) throw new CoronaDL.Exceptions.TruckAlreadyExistsException();
            foodtruck.EndDelivery = QuarterOnly(foodtruck.EndDelivery);
            foodtruck.StartDelivery = QuarterOnly(foodtruck.StartDelivery);
            foodtruck.StartOrder = QuarterOnly(foodtruck.StartOrder);
            if (foodtruck.EndDelivery <= foodtruck.StartDelivery) throw new Exception("Das End-Uhrzeit muss später sein als die Start-Uhrzeit");

            _dbFoodTruck.Insert(foodtruck);
            CreateDefaultSchedule(foodtruck);
        }

        public IEnumerable<FoodTruck> GetAllTrucks(bool onlyActive = true)
        {
            return _dbFoodTruck.SelectAll().Where(q => q.Active == true || onlyActive);
        }

        public IEnumerable<Position> GetCard(Guid truckId)
        {
            return _dbPosition.GetForTruck(truckId);
        }

        public IEnumerable<Place> GetPlacesForTruck(Guid truckId)
        {
            return _dbPlace.GetForFoodtruck(truckId);
        }

        public IEnumerable<Schedule> GetSchedule(Guid truckId)
        {
            return _dbSchedule.GetForFoodtruck(truckId);
        }

        public IEnumerable<Slot> GetSlots(Guid scheduleId)
        {
            return _dbSlot.GetForSchedule(scheduleId);
        }

        public FoodTruck GetTruck(Guid truckId)
        {
            return _dbFoodTruck.SelectAll().FirstOrDefault(q => q.Id == truckId);
        }

        private DateTime QuarterOnly(DateTime dateTime)
        {
            var minute = dateTime.Minute;
            if (minute % 15 != 0) minute = 0;
            return new DateTime(0, 0, 0, dateTime.Hour, minute, 0);
        }

        public void InsertPlace(Place place)
        {
            if (place.TruckId == null) throw new Exception();
            CheckPermission(place.TruckId);

            _dbPlace.Insert(place);
        }

        private void CreateWeekSchedule(bool evenWeek, FoodTruck truck)
        {
            _dbSchedule.DeleteOldSchedule(truck.Id);
            for (DayOfWeek weekday = DayOfWeek.Sunday; weekday <= DayOfWeek.Saturday; weekday++)
            {
                var weekdaySchedule = new CoronaEntities.Schedule
                {
                    IsEven = evenWeek,
                    PlaceId = null,
                    TruckId = truck.Id,
                    Weekday = weekday
                };
                _dbSchedule.InsertSchedule(weekdaySchedule);
                _dbSlot.DeleteForSchedule(weekdaySchedule.Id);
                var currentTime = truck.StartDelivery;
                while (currentTime <= truck.EndDelivery)
                {
                    var slot = new Slot
                    {
                        IsOpen = true,
                        ScheduleId = weekdaySchedule.Id,
                        SlotTime = currentTime,
                        TruckId = truck.Id
                    };
                    _dbSlot.Insert(slot);
                }
            }
        }

        private void CreateDefaultSchedule(FoodTruck foodTruck)
        {
            CreateWeekSchedule(true, foodTruck);
            CreateWeekSchedule(false, foodTruck);
        }

        public void StartDay(Guid truckId)
        {
            var dayOfWeek = DateTime.Now.DayOfWeek;
            bool isEven = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday) % 2 == 0;

            var schedules = _dbSchedule.GetForFoodtruck(truckId);
            var todaySchedule = schedules.FirstOrDefault(q => q.IsEven == isEven && q.Weekday == dayOfWeek);
        }

        public void UpdateCard(Guid truckId, IEnumerable<Position> positions)
        {
            if (positions.FirstOrDefault(q => q.TruckId != truckId) != null) throw new Exception("Wrong data");
            CheckPermission(truckId);
            _dbPosition.DeleteForTruck(truckId);
            foreach (var position in positions)
            {
                _dbPosition.Insert(position);
            }
        }

        public void UpdatePlace(Place place)
        {
            CheckPermission(place.TruckId);
            _dbPlace.Update(place);
        }

        public void UpdateSchedule(Guid truckId, IEnumerable<Schedule> schedule)
        {
            if (schedule.FirstOrDefault(q => q.TruckId != truckId) != null) throw new Exception("Wrong data");
            CheckPermission(truckId);
            _dbSchedule.DeleteOldSchedule(truckId);
            foreach (var scheduleItem in schedule)
            {
                _dbSchedule.InsertSchedule(scheduleItem);
            }
        }

        public void UpdateSlots(Guid scheduleId, IEnumerable<Slot> slots)
        {
            var schedule = _dbSchedule.GetById(scheduleId);
            if (slots.FirstOrDefault(q => q.ScheduleId != scheduleId) == null) throw new Exception("Wrong data");
            CheckPermission(schedule.TruckId);

            _dbSlot.UpdateForSchedule(slots);
        }

        public void UpdateTruck(FoodTruck foodtruck)
        {
            _dbFoodTruck.Update(foodtruck);
        }

     
    }
}
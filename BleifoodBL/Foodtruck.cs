using Bleifood.BL.Interfaces;

using Bleifood.Entities;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Bleifood.BL
{
    public class FoodTruck : IFoodTruck
    {
        private readonly DL.Interfaces.IFoodTruck _dbFoodTruck = new DL.FoodTruck();
        private readonly DL.Interfaces.IPlace _dbPlace = new DL.Place();
        private readonly DL.Interfaces.IPosition _dbPosition = new DL.Position();
        private readonly DL.Interfaces.ISchedule _dbSchedule = new Bleifood.DL.Schedule();
        private readonly DL.Interfaces.ISlot _dbSlot = new DL.Slot();

        private bool CheckPermission(Guid? truckId)
        {
            return false;
        }

        private string CreateRandomTestHash()
        {
            return Helpers.CreateRandomString(8);
        }

        public void CreateTruck(Entities.FoodTruck foodtruck)
        {
            CheckPermission(null);
            var existingTruck = GetAllTrucks().FirstOrDefault(q => q.Url.Equals(foodtruck.Url, StringComparison.InvariantCultureIgnoreCase));
            if (existingTruck != null) throw new Bleifood.DL.Exceptions.TruckAlreadyExistsException();
            foodtruck.EndDelivery = QuarterOnly(foodtruck.EndDelivery);
            foodtruck.StartDelivery = QuarterOnly(foodtruck.StartDelivery);
            foodtruck.StartOrder = QuarterOnly(foodtruck.StartOrder);
            foodtruck.InDataBase = true;
            foodtruck.TestToken = CreateRandomTestHash();
            _dbFoodTruck.Insert(foodtruck);
            CreateDefaultSchedule(foodtruck);
        }

        public IEnumerable<Entities.FoodTruck> GetAllTrucks(bool onlyActive = true)
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
            return _dbSlot.GetForSchedule(scheduleId).OrderBy(q => q.SlotTime);
        }

 
        public Entities.FoodTruck GetTruck(Guid truckId)
        {
            return _dbFoodTruck.SelectAll().FirstOrDefault(q => q.Id == truckId);
        }

        private Time QuarterOnly(Time time)
        {
            var minute = time.Minute;
            if (minute % 15 != 0) minute = 0;
            return new Time(time.Hour, minute);
        }

        public void InsertPlace(Place place)
        {
            if (place.TruckId == null) throw new Exception();
            CheckPermission(place.TruckId);

            _dbPlace.Insert(place);
        }

        private void CreateWeekSchedule(bool evenWeek, Entities.FoodTruck truck)
        {
            
            for (DayOfWeek weekday = DayOfWeek.Sunday; weekday <= DayOfWeek.Saturday; weekday++)
            {
                var weekdaySchedule = new Schedule
                {
                    IsEven = evenWeek,
                    PlaceId = null,
                    TruckId = truck.Id,
                    Weekday = weekday
                };
                _dbSchedule.InsertSchedule(weekdaySchedule);
            }
        }

        private void CreateDefaultSchedule(Entities.FoodTruck foodTruck)
        {
            CreateWeekSchedule(true, foodTruck);
            CreateWeekSchedule(false, foodTruck);
        }

        //public void StartDay(Guid truckId)
        //{
        //    var dayOfWeek = DateTime.Now.DayOfWeek;
        //    bool isEven = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday) % 2 == 0;

        //    var schedules = _dbSchedule.GetForFoodtruck(truckId);
        //    var todaySchedule = schedules.FirstOrDefault(q => q.IsEven == isEven && q.Weekday == dayOfWeek);
        //}

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

        public void UpdateTruck(Entities.FoodTruck foodtruck)
        {
            foodtruck.InDataBase = true;
            _dbFoodTruck.Update(foodtruck);
        }

        public Guid? GetTruckFromUser(string userId)
        {
            if (userId == null) throw new Exception("Unknown user");
            var myTruck = GetAllTrucks().FirstOrDefault(q => q.UserId == userId);
            if (myTruck == null) return null;
            return myTruck.Id;
        }

        private string BuildAddress(Place place)
        {
            return $"{place.Road}, {place.City}";
        }

        private async Task<GeoCoordinate> UpdateLocationForPlace(Place place)
        {
            Geocode geocode = new Geocode();
            var coords= await geocode.GetCoordinates(BuildAddress(place));
            return coords;
        }

        public async void UpdatePlacesForTruck(Guid truckId, IEnumerable<Place> places)
        {
            if (places.Any(q => q.TruckId != truckId)) throw new Exception("you are at the wrong place, stranger");
            var oldPlaces = GetPlacesForTruck(truckId);
            var addedPlaces = new List<Guid>();
            var deletedPlaces = new List<Guid>();
            foreach (var place in places)
            {
                var coords=await  UpdateLocationForPlace(place);
                place.Coordinates = coords;

            }
            foreach (var oldPlace in oldPlaces) {
                if (!places.Any(q => q.Id == oldPlace.Id)) deletedPlaces.Add(oldPlace.Id);
            }
            foreach (var newPlace in places)
            {
                if (!oldPlaces.Any(q => q.Id == newPlace.Id)) addedPlaces.Add(newPlace.Id);
            }

            foreach (var deletedPlace in deletedPlaces)
            {
                _dbPlace.Delete(deletedPlace);
            }
            foreach (var addedPlace in places.Where(q=>addedPlaces.Contains(q.Id)))
            {
                _dbPlace.Insert(addedPlace);
            }
            foreach (var modifiedPlace in places.Where(q=>!addedPlaces.Contains(q.Id)))
            {
                _dbPlace.Update(modifiedPlace);
            }
        }
    }
}
using CoronaBL.Interfaces;
using CoronaEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaBL
{
    public class Foodtruck : IFoodtruck
    {
        private CoronaDL.Interfaces.IFoodTruck _dbFoodTruck = new CoronaDL.FoodTruck();
        private Interfaces.IUser _user;

        private bool CheckPermission(Guid? truckId)
        {
            var currentUser = _user.GetFromCookie();
            if (currentUser == null) return false;
            if (truckId == null) return true;
            return currentUser.TruckId == truckId;
        }

        public void CreateTruck(FoodTruck foodtruck)
        {
            CheckPermission(null);
            var existingTruck = GetAllTrucks().FirstOrDefault(q => q.Url.Equals(foodtruck.Url, StringComparison.InvariantCultureIgnoreCase));
            if (existingTruck != null) throw new CoronaDL.Exceptions.TruckAlreadyExistsException();
            _dbFoodTruck.Insert(foodtruck);
            // TODO: Create Default Schedule
        }

        public IEnumerable<FoodTruck> GetAllTrucks(bool onlyActive = true)
        {
            return _dbFoodTruck.SelectAll().Where(q => q.Active == true || onlyActive);
        }

        public IEnumerable<Position> GetCard(Guid truckId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Place> GetPlacesForTruck(Guid truckId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Schedule> GetSchedule(Guid truckId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Slot> GetSlots(Guid truckId, Guid scheduleId)
        {
            throw new NotImplementedException();
        }

        public FoodTruck GetTruck(Guid truckId)
        {
            return _dbFoodTruck.SelectAll().FirstOrDefault(q => q.Id == truckId);
        }

        public void InsertPlace(Place place)
        {
            throw new NotImplementedException();
        }

        public void StartDay(Guid truckId)
        {
            throw new NotImplementedException();
        }

        public void UpdateCard(IEnumerable<Position> positions)
        {
            throw new NotImplementedException();
        }

        public void UpdatePlace(Place place)
        {
            throw new NotImplementedException();
        }

        public void UpdateSchedule(Guid truckId, IEnumerable<Schedule> schedule)
        {
            throw new NotImplementedException();
        }

        public void UpdateSlots(Guid truckId, Guid scheduleId, IEnumerable<Slot> slots)
        {
            throw new NotImplementedException();
        }

        public void UpdateTruck(FoodTruck foodtruck)
        {
            _dbFoodTruck.Update(foodtruck);
        }
    }
}

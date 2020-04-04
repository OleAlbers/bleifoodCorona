using System;
using System.Collections.Generic;

namespace Bleifood.BL.Interfaces
{
    public interface IFoodTruck
    {
        void CreateTruck(Entities.FoodTruck foodtruck);

        Entities.FoodTruck GetTruck(Guid truckId);

        IEnumerable<Entities.FoodTruck> GetAllTrucks(bool onlyActive = true);

        void UpdateTruck(Entities.FoodTruck foodtruck);

        IEnumerable<Entities.Place> GetPlacesForTruck(Guid truckId);

        void UpdatePlacesForTruck(Guid truckId, IEnumerable<Entities.Place> places);

        void InsertPlace(Entities.Place place);

        void UpdatePlace(Entities.Place place);

        IEnumerable<Entities.Position> GetCard(Guid truckId);

        void UpdateCard(Guid truckId, IEnumerable<Entities.Position> positions);

        IEnumerable<Entities.Schedule> GetSchedule(Guid truckId);

        void UpdateSchedule(Guid truckId, IEnumerable<Entities.Schedule> schedule);

        IEnumerable<Entities.Slot> GetSlots( Guid scheduleId);


        Guid? GetTruckFromUser(string userId);

        IEnumerable<Entities.FoodTruck> GetNearbyTrucks(Entities.GeoCoordinate coordinate, Guid? myTruckId);
    }
}
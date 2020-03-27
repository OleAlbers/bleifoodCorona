using System;
using System.Collections.Generic;

namespace Bleifood.BL.Interfaces
{
    public interface IFoodtruck
    {
        void CreateTruck(CoronaEntities.FoodTruck foodtruck);

        CoronaEntities.FoodTruck GetTruck(Guid truckId);

        IEnumerable<CoronaEntities.FoodTruck> GetAllTrucks(bool onlyActive = true);

        void UpdateTruck(CoronaEntities.FoodTruck foodtruck);

        IEnumerable<CoronaEntities.Place> GetPlacesForTruck(Guid truckId);

        void InsertPlace(CoronaEntities.Place place);

        void UpdatePlace(CoronaEntities.Place place);

        IEnumerable<CoronaEntities.Position> GetCard(Guid truckId);

        void UpdateCard(Guid truckId, IEnumerable<CoronaEntities.Position> positions);

        IEnumerable<CoronaEntities.Schedule> GetSchedule(Guid truckId);

        void UpdateSchedule(Guid truckId, IEnumerable<CoronaEntities.Schedule> schedule);

        IEnumerable<CoronaEntities.Slot> GetSlots( Guid scheduleId);

        void UpdateSlots( Guid scheduleId, IEnumerable<CoronaEntities.Slot> slots);

        void StartDay(Guid truckId);    // Reset slots etc.
    }
}
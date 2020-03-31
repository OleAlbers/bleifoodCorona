
namespace Bleifood.Entities
{
    // Geocode is EXPENSIVE so lets cache as max as possible
    public class GeocodeCache : BaseEntity
    {
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
using System;

namespace CoronaEntities
{
    public class User : BaseEntity
    {
        public Guid? TruckId { get; set; }
        public string Password { get; set; }
        public DateTime? Validated { get; set; }
        public string ValidationCode { get; set; }
        public DateTime HashValidUntil { get; set; }
        public Credentials Credentials { get; set; } = new Credentials();
    }
}
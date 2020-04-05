

namespace Bleifood.Entities
{
    public class Customer : BaseEntity
    {
        public Address PostAddress { get; set; } = new Address();
    }
}

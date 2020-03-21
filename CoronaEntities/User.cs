using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaEntities
{
    public class User : BaseEntity
    {
        public Guid?     TruckId { get; set; }
        public string LoginMail { get; set; }
        public string Password { get; set; }
        public DateTime? Validated { get; set; }
        public string ValidationCode { get; set; }
        public string Hash { get; set; }
        public DateTime HashValidUntil { get; set; }
    }
}

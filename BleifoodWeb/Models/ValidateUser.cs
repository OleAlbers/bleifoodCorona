using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bleifood.Web.Models
{
    public class ValidateUser
    {
        public bool IsValid { get; set; }
        public string LastError { get; set; }
        public string UserId { get; set; }
        public string Hash { get; set; }
    }
}

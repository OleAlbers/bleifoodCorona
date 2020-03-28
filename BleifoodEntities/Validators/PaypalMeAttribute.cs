using System;
using System.Collections.Generic;
using System.Text;

namespace Bleifood.Entities.Validators
{
    public class PaypalMeAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute
    {
        public PaypalMeAttribute() : base("https:\\/\\/(www\\.)?(paypal.me\\/).*")
        {

        }
    }
}

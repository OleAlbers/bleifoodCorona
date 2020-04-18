using System;
using System.Collections.Generic;
using System.Text;

namespace Bleifood.Entities.Validators
{
    public class RangeAttribute : System.ComponentModel.DataAnnotations.RangeAttribute
    {
        public RangeAttribute(double minimum, double maximum) : base(minimum, maximum)
        {

        }

        public override string FormatErrorMessage(string name)
        {
            return $"Der Wert muss zwischen {Minimum} und {Maximum} liegen";
        }
    }
}

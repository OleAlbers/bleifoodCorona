using System;
using System.Collections.Generic;
using System.Text;

namespace Bleifood.Entities.Validators
{
    public class RegularExpressionAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute
    {
        public RegularExpressionAttribute(string pattern) : base(pattern)
        {
        }
        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Bleifood.Entities.Validators
{
    public class RequiredAttribute:System.ComponentModel.DataAnnotations.RequiredAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return "Dies ist ein Pflichtfeld.";
        }
    }
}

using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bleifood.Web
{
    public class InputSelectNumber<T> : InputSelect<T>
    {
        protected override bool TryParseValueFromString(string value, out T result, out string validationErrorMessage)
        {
            validationErrorMessage = null;
            try
            {
                result = (T)Convert.ChangeType(value, typeof(T));
                return true;
            }
            catch (Exception)
            {
                result = default(T);
                validationErrorMessage = "wrong type";
                return false;
            }
        }
    }
}

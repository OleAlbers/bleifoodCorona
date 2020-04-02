using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                var type = typeof(T);
                if (value == null)
                {
                    result = default;
                    return true;
                }
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = type.GetGenericArguments()[0];
                }
                if (type==typeof(Guid))
                {
                    result=  (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(value);
                    return true;
                }
                result = (T)Convert.ChangeType(value, type);
                return true;
            }
            catch (Exception ex)
            {
                result = default;
                validationErrorMessage = "wrong type";
                return false;
            }
        }
    }
}

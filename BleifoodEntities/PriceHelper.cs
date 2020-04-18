using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Bleifood.Entities
{
    public static class PriceHelper
    {
        public static string FromCurrency(this decimal value)
        {
             return string.Format(CultureInfo.GetCultureInfo("de-de"), "{0:0.00}", value);
        }

        public static decimal ToCurrency(this string value)
        { 

            if (decimal.TryParse(value, NumberStyles.AllowDecimalPoint| NumberStyles.AllowCurrencySymbol,CultureInfo.GetCultureInfo("de-de"), out decimal decValue))
            {
                return decValue;
            }
            return 0;
        }
    }
}

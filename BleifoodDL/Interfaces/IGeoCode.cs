using System;
using System.Collections.Generic;
using System.Text;

namespace Bleifood.DL.Interfaces
{
    public interface IGeoCode
    {
        void StoreCode(Entities.GeocodeCache geocodeCache);
        Entities.GeocodeCache GetForAddress(string address);
    }
}

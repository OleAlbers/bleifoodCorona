using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaBL.Interfaces
{
    public interface ICustomer
    {
        CoronaEntities.Customer GetFromCookie();
        void StoreInCookie(CoronaEntities.Customer customer);
    }
}

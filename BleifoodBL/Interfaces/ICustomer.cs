using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bleifood.BL.Interfaces
{
    public interface ICustomer
    {
        Bleifood.Entities.Customer GetFromLocalStorage();
        void StoreInLocalStorage(Bleifood.Entities.Customer customer);
    }
}

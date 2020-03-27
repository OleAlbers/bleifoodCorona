using Bleifood.BL.Interfaces;

using System;

namespace Bleifood.BL
{
    public class Customer : ICustomer
    {

        private IBrowserStorage _localStorage;

        public CoronaEntities.Customer GetFromLocalStorage()
        {
            return null;
            //var address = _localStorage.ReadData<CoronaEntities.Address>().Result;
            //if (address == null) return null;
            //return new CoronaEntities.Customer { PostAddress = address };
        }

        public void StoreInLocalStorage(CoronaEntities.Customer customer)
        {
            _localStorage.StoreData(customer.PostAddress);
        }
    }
}
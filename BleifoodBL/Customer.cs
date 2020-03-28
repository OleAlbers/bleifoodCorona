using Bleifood.BL.Interfaces;

using System;

namespace Bleifood.BL
{
    public class Customer : ICustomer
    {

        private IBrowserStorage _localStorage;

        public Bleifood.Entities.Customer GetFromLocalStorage()
        {
            return null;
            //var address = _localStorage.ReadData<Bleifood.Entities.Address>().Result;
            //if (address == null) return null;
            //return new Bleifood.Entities.Customer { PostAddress = address };
        }

        public void StoreInLocalStorage(Bleifood.Entities.Customer customer)
        {
            _localStorage.StoreData(customer.PostAddress);
        }
    }
}
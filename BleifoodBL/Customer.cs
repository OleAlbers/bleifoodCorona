using CoronaBL.Interfaces;

using System;

namespace CoronaBL
{
    public class Customer : ICustomer
    {

        private ILocalStorage _localStorage;

        public Customer(ILocalStorage localStorage)
        {
            _localStorage= localStorage;
        }

        public CoronaEntities.Customer GetFromLocalStorage()
        {
            var address= _localStorage.ReadData<CoronaEntities.Address>();
            if (address == null) return null;
            return new CoronaEntities.Customer { PostAddress = address };
        }

        public void StoreInLocalStorage(CoronaEntities.Customer customer)
        {
            _localStorage.StoreData(customer.PostAddress);
        }
    }
}
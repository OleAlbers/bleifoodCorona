using CoronaBL.Interfaces;

using System;

namespace CoronaBL
{
    public class Customer : ICustomer
    {

        private ICookies _cookies;

        public Customer(ICookies cookies)
        {
            _cookies = cookies;
        }

        public CoronaEntities.Customer GetFromCookie()
        {
            var customer = _cookies.GetCookie<CoronaEntities.Customer>();
            if (customer != null) StoreInCookie(customer);  // Refresh
            return customer;
        }

        public void StoreInCookie(CoronaEntities.Customer customer)
        {
            _cookies.SetCookie(customer, new TimeSpan(100, 0, 0, 0));
        }
    }
}
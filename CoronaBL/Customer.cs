using CoronaBL.Interfaces;

using System;

namespace CoronaBL
{
    public class Customer : ICustomer
    {
        public CoronaEntities.Customer GetFromCookie()
        {
            var customer = Cookies.GetCookie<CoronaEntities.Customer>();
            if (customer != null) StoreInCookie(customer);  // Refresh
            return customer;
        }

        public void StoreInCookie(CoronaEntities.Customer customer)
        {
            Cookies.SetCookie(customer, new TimeSpan(100, 0, 0, 0));
        }
    }
}
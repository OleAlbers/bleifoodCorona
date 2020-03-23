using System;

namespace CoronaBL.Interfaces
{
    public interface IUser
    {
        Guid? Login(string mail, string password);
        void Register(string mail, string password);
        void Activate(string mail, string hash);
        void ChangePassword(string mail, string passwordOld, string passwordnew);
        CoronaEntities.User GetFromCookie();
    }
}
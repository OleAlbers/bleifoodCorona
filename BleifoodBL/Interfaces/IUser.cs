using System;
using System.Threading.Tasks;

namespace CoronaBL.Interfaces
{
    public interface IUser
    {
        Task<bool> Login(string mail, string password);

        void Register(string mail, string password);

        void LogOut();

        void ConfirmAccount(string mail, string code);
    }

}
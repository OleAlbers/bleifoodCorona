using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BlazorFood.UserApi;

namespace BlazorFood
{
    public interface IUserApi
    {
        void Register(string mail, string password);
        Task<bool> Login(string mail, string password);
        void Validate(string mail, string hash);
    }
}

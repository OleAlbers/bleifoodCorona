using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorFood
{
    public interface IUserApi
    {
        void Register(Models.RegisterUser registerdata);
        bool Login(ILocalStorageService localStorageService, string mail, string password, out string error, out Guid? guid);
        bool Validate(string mail, string hash, out string error);
    }
}

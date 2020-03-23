using Blazored.LocalStorage;
using CoronaBL;
using CoronaBL.Interfaces;
using log4net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorFood
{
    public class UserApi : IUserApi
    {
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        

        
        public void Register(Models.RegisterUser registerdata)
        {
            IUser user = new User(null);
            user.Register(registerdata.Mail, registerdata.Password);
            _log.Debug("User registered");
        }

        public bool Login(ILocalStorageService localStorageService,  string mail, string password, out string error, out Guid? guid)
        {
            error = null;
            guid= null;
            try
            {
                IUser user = new User(localStorageService);
                guid = user.Login(mail, password);
                
                return true;
            }
            catch (Exception ex)
            {
                error = ex.ToString();
                return false;
            }
        }

        public bool Validate(string mail, string hash, out string error)
        {
            // TODO: Catch specific errors and show useful errors
            error = null;
            try
            {
                IUser user = new User(null);
                user.Activate(mail, hash);
                return true;
            }
            catch (Exception ex)
            {
                error = ex.ToString();
                return false;
            }
        }
    }
}

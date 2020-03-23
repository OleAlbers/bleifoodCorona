using CoronaBL;
using CoronaBL.Interfaces;
using log4net;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorFood
{
    public class UserApi:IUserApi
    {
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        IUser _user;

        public UserApi(IUser user)
        {
            _user = user;
        }
        public void Register(Models.RegisterUser registerdata)
        {
            _user.Register(registerdata.Mail, registerdata.Password);
            _log.Debug("User registered");
        }

        public bool Login(string mail, string password, out string error, out Guid? guid)
        {
            error = null;
            guid= null;
            try
            {
                guid = _user.Login(mail, password);
                
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
                _user.Activate(mail, hash);
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

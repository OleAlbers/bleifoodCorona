using AspNetCore.Identity.LiteDB.Models;
using CoronaBL;
using CoronaBL.Interfaces;
using CoronaDL.Exceptions;
using log4net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
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

        private readonly IUser _user;

        public UserApi(IUser user)
        {
            _user = user;
        }

        public async Task<bool> Login(string mail, string password)
        {
            return await _user.Login(mail, password);
        }

        public void Register(string mail, string password)
        {
            _user.Register(mail, password);
        }

    

        public void Validate(string mail, string hash)
        {
                _user.ConfirmAccount(mail, hash);
        }
    }
}

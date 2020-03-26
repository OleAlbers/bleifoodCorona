using AspNetCore.Identity.LiteDB.Models;
using CoronaBL.Interfaces;
using log4net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CoronaBL
{
    public class User : IUser
    {
        private SignInManager<ApplicationUser> _signinManager;
        private UserManager<ApplicationUser> _userManager;

        private IBrowserStorage _browserStorage;
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private TimeSpan LocalStoregeDuration = new TimeSpan(1, 0, 0);  // TODO: From Config

        private IMail _mail = new Mail();
        private CoronaDL.Interfaces.IUser _dbUser = new CoronaDL.User();
        private NavigationManager _navigationManager;

        public User(IBrowserStorage browserStorage, NavigationManager navigationManager, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _browserStorage = browserStorage;
            _navigationManager = navigationManager;
            _signinManager = signInManager;
            _userManager = userManager;
        }


        public async Task<bool> Login(string mail, string password)
        {
            var result = await _signinManager.PasswordSignInAsync(mail, password, false, false);
            return result.Succeeded;
        }


    

        public async void Register(string mail, string password)
        {
            var user = new ApplicationUser { Email = mail, UserName = mail };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded) return;
            if (result.Errors.Any()) throw new Exception(result.Errors.First().Description);
        }

        public void LogOut()
        {
            _signinManager.SignOutAsync();
        }

        public async void ConfirmAccount(string mail, string code)
        {
            var user = await _userManager.FindByEmailAsync(mail);
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
        }
    }
}

using AspNetCore.Identity.LiteDB.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BleifoodWeb
{
    public class Identity
    {
        private SignInManager<ApplicationUser> _signinManager;
        private UserManager<ApplicationUser> _userManager;

        public Identity(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signinManager = signInManager;
            _userManager = userManager;
        }

        public void Register()
        {
            var newUser = new ApplicationUser { Email = "test@whatever.com", UserName = "test@whatever.com" };
            var registerReslut = _userManager.CreateAsync(newUser, "password");
        }

        public void Login()
        {
            var loginResult=_signinManager.PasswordSignInAsync("test@whatever.com", "password", false, false).Result;
        }
    }
}

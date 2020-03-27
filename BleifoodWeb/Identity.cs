using AspNetCore.Identity.LiteDB.Models;
using Bleifood.BL.Interfaces;
using Bleifood.Web.Models;
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
        private IMail _mail;

        public Identity(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMail mail)
        {
            _signinManager = signInManager;
            _userManager = userManager;
            _mail = mail;
        }

        public async Task<bool> Register(RegisterUser regUser)
        {
            if (!regUser.Password.Equals(regUser.PasswordRepeat)) throw new Exception("Passwörter sind unterschiedlich");
            var newUser = new ApplicationUser { Email = regUser.Mail, UserName = regUser.Mail};
            
            var sd= await _userManager.CreateAsync(newUser, "password");
            if (!sd.Succeeded)
            {
                regUser.LastError = "Unbekannter Fehler";
                if (sd.Errors != null && sd.Errors.Count() > 0) regUser.LastError = sd.Errors.FirstOrDefault().Description;
                return false;
            }

            var token=await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            _mail.Validate(newUser,token);
            return true;
        }

        public async Task<bool> Validate(ValidateUser user)
        {
            user.IsValid = false;
            var storedUser = await _userManager.FindByIdAsync(user.UserId);
            if (storedUser==null)
            {
                user.LastError = "Unbekannter User";
                return false;
            }
            if (storedUser.EmailConfirmed)
            {
                user.LastError = "Bereits validiert";
                return false;
            }
            if (storedUser.AuthenticationKey!=user.Hash)
            {
                user.LastError = "Ungültige Daten";
                return false;
            }
            storedUser.EmailConfirmed = true;
            user.IsValid = true;
            return true;
        }

        public void Login()
        {
            var loginResult=_signinManager.PasswordSignInAsync("test@whatever.com", "password", false, false).Result;
        }
    }
}

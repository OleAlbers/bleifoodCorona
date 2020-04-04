using AspNetCore.Identity.LiteDB.Models;
using Bleifood.BL.Interfaces;
using Bleifood.Web.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace Bleifood.Web
{
    public class Identity
    {
        private SignInManager<ApplicationUser> _signinManager;
        private UserManager<ApplicationUser> _userManager;
        private NavigationManager _navigationManager;
        private AuthenticationStateProvider _authenticationStateProvider;
        private IMail _mail;

        public Identity(AuthenticationStateProvider authenticationStateProvider, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMail mail, NavigationManager navigationManager)
        {
            _signinManager = signInManager;
            _userManager = userManager;
            _navigationManager = navigationManager;
            _authenticationStateProvider = authenticationStateProvider;
            _mail = mail;
        }

        public async Task<bool> Register(RegisterUser regUser)
        {
            if (!regUser.Password.Equals(regUser.PasswordRepeat)) throw new Exception("Passwörter sind unterschiedlich");
            var newUser = new ApplicationUser { Email = regUser.Mail, UserName = regUser.Mail };

            var sd = await _userManager.CreateAsync(newUser, regUser.Password);
            if (!sd.Succeeded)
            {
                regUser.LastError = "Unbekannter Fehler";
                if (sd.Errors != null && sd.Errors.Count() > 0) regUser.LastError = sd.Errors.FirstOrDefault().Description;
                return false;
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            _mail.Validate(newUser, token);
            return true;
        }


        
        public async Task<bool> Validate(ValidateUser user)
        {
            var storedUser = await _userManager.FindByIdAsync(user.UserId);
            if (storedUser == null || storedUser.Id != user.UserId)
            {
                user.LastError = "Unbekannter User";
                return false;
            }
            if (storedUser.EmailConfirmed)
            {
                // Already Confirmed. Second Call most likely
                return true;
            }
            var confirmResult = await _userManager.ConfirmEmailAsync(storedUser, user.Hash);
            if (confirmResult.Succeeded)
            {
                storedUser.EmailConfirmed = true;
                await _userManager.UpdateAsync(storedUser);
                return true;
            }
            user.LastError = confirmResult.Errors.FirstOrDefault().Description;
            return false;
        }

        public async Task<SignInResult> Login(string user, string pass)
        {
            return await _signinManager.PasswordSignInAsync(user, pass, false, false);
        }

        public async Task<bool> CanLogin(string user, string pass)
        {
            var applicationUser = await GetByMail(user);
            if (applicationUser == null) return false;
            var checkResult = await _signinManager.CheckPasswordSignInAsync(applicationUser, pass, false);
            return checkResult.Succeeded;
        }

        public async Task<ApplicationUser> GetById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<ApplicationUser> GetByMail(string mail)
        {
            return await _userManager.FindByEmailAsync(mail);
        }

        public async Task<bool> IsLoggedIn()
        {
            try
            {
                var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
                var claimsPrinciple = state.User;
                var isSignedIn = _signinManager.IsSignedIn(claimsPrinciple);
                return isSignedIn;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Guid?> GetMyTruckId()
        {
            var currentUser = await GetCurrentUser();
            IFoodTruck truckLogic = new BL.FoodTruck();
            return truckLogic.GetTruckFromUser(currentUser.Id);            
        }

        public async Task<ApplicationUser> GetCurrentUser()
        {
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var claimsPrinciple = state.User;
            if (claimsPrinciple.Identity.IsAuthenticated)
            {
                var currentUser = await _userManager.GetUserAsync(claimsPrinciple);
                return currentUser;
            }
            return null;
        }

        public async void LogOut()
        {
            await _signinManager.SignOutAsync();
        }

    }
}

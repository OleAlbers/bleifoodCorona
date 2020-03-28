using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bleifood.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bleifood.Web
{
    [Route("api/login")]
    public class LoginController : Controller
    {
        private Identity _identity;
        public LoginController(Identity identity)
        {
            _identity = identity;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]LoginUser user)
        {
            try
            {
                bool canLogin = await _identity.CanLogin(user.Mail, user.Password);
                if (!canLogin) return StatusCode(401);
                    var identityUser = await _identity.GetByMail(user.Mail);
                    if (!identityUser.EmailConfirmed) return StatusCode(420);
                var signinResult = await _identity.Login(user.Mail, user.Password);
                if (signinResult.IsLockedOut) return StatusCode(449);
                if (signinResult.IsNotAllowed) return StatusCode(403);
                if (!signinResult.Succeeded) return StatusCode(401);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                // TODO: Log
                return StatusCode(500);
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            try
            {
                _identity.LogOut();
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                // TODO: Log
                return StatusCode(500);
            }
        }
    }
}

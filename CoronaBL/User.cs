using log4net;
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
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string UserCookie = "BleifoodUsr";
        private const string UserCookieId = "Id";
        private const string UserCookeHash = "Hash";
        private TimeSpan UserCookieDuration = new TimeSpan(1, 0, 0);

        private IMail _mail;
        private CoronaDL.IUser _dbUser = new CoronaDL.User();

        public void Activate(string mail, string validation)
        {
            var existingUser = _dbUser.SelectByMail(mail);
            if (existingUser == null || existingUser.ValidationCode != validation || string.IsNullOrWhiteSpace(validation)) throw new CoronaDL.Exceptions.InvalidHashException();
            if (existingUser.Validated != null) throw new CoronaDL.Exceptions.AlreadyValidatedException();
            existingUser.ValidationCode = null;
            existingUser.Validated = DateTime.Now;
            _dbUser.Update(existingUser);
        }

        public void ChangePassword(string mail, string passwordOld, string passwordnew)
        {
            var existingUser = _dbUser.SelectByMail(mail);
            if (existingUser == null || Hash.CreateHash(passwordOld) != existingUser.Password) throw new CoronaDL.Exceptions.WrongCredentialsException();
            existingUser.Password = Hash.CreateHash(passwordnew);
            _dbUser.Update(existingUser);
        }

        public void Login(string mail, string password)
        {
            var existingUser = _dbUser.SelectByMail(mail);
            if (existingUser == null || Hash.CreateHash(password) != existingUser.Password) throw new CoronaDL.Exceptions.WrongCredentialsException();
            if (existingUser.Validated == null) throw new CoronaDL.Exceptions.NotValidatedException();
            SetCookie(existingUser, false);
        }

        public void SetCookie(CoronaEntities.User user, bool renew)
        {
            if (user == null)
            {
                _log.Error($"usr is null");
                return;
            }

            if (!renew) user.Hash = CreateRandomHash();
            HttpCookie userInfo = new HttpCookie(UserCookie);
            userInfo.HttpOnly = true;
            userInfo.Secure = true;
            userInfo[UserCookieId] = user.Id.ToString();
            userInfo[UserCookeHash] = user.Hash;
            userInfo.Expires.Add(UserCookieDuration);
            HttpContext.Current.Response.Cookies.Add(userInfo);
            user.HashValidUntil = DateTime.Now.Add(UserCookieDuration);
            _dbUser.Update(user);
        }

        private CoronaEntities.User GetUserFromCookie()
        {
            var cookie = HttpContext.Current.Request.Cookies[UserCookie];
            if (cookie == null) return null;
            var cookieId = Guid.Parse(cookie[UserCookieId]);
            string cookieHash = cookie[UserCookeHash];
            var usr = _dbUser.GetAll().FirstOrDefault(q => q.Id == cookieId && q.Hash == cookieHash);
            if (usr == null) return null;
            if (usr.HashValidUntil < DateTime.Now) return null;  // Hash expired

            SetCookie(usr,true); 
            return usr;
        }

        private string CreateRandomHash()
        {
            return Hash.CreateHash(Guid.NewGuid().ToString()); // TODO: Could be more fancy
        }

        public void Register(string mail, string password)
        {
            var existingUser = _dbUser.SelectByMail(mail);
            if (existingUser != null) throw new CoronaDL.Exceptions.UserAlreadyExistsException();

            var newUser = new CoronaEntities.User
            {
                Created = DateTime.Now,
                Id = Guid.NewGuid(),
                LoginMail = mail,
                Password = Hash.CreateHash(password),
                ValidationCode = Guid.NewGuid().ToString()
            };
            _dbUser.Insert(newUser);
            _mail.Validate(newUser);
        }
    }
}

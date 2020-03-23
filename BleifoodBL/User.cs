using CoronaBL.Interfaces;
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
        private ICookies _cookies;
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private TimeSpan UserCookieDuration = new TimeSpan(1, 0, 0);

        private IMail _mail = new Mail();
        private CoronaDL.Interfaces.IUser _dbUser = new CoronaDL.User();

        public User(ICookies cookies)
        {
            _cookies = cookies;
        }

        public void Activate(string mail, string validation)
        {
            var existingUser = _dbUser.SelectByMail(mail);
            if (existingUser.Validated != null) return; // already validated
            if (existingUser == null || existingUser.ValidationCode != validation || string.IsNullOrWhiteSpace(validation)) throw new CoronaDL.Exceptions.InvalidHashException();
            
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

        public Guid? Login(string mail, string password)
        {
            var existingUser = _dbUser.SelectByMail(mail);
            if (existingUser == null || Hash.CreateHash(password) != existingUser.Password) throw new CoronaDL.Exceptions.WrongCredentialsException();
            if (existingUser.Validated == null) throw new CoronaDL.Exceptions.NotValidatedException();
            existingUser.Hash = CreateRandomHash();
            existingUser.HashValidUntil = _cookies.SetCookie(existingUser, UserCookieDuration);
            _dbUser.Update(existingUser);
            return existingUser.TruckId;
        }

        public CoronaEntities.User GetFromCookie()
        {
            var cookieUser = _cookies.GetCookie<CoronaEntities.User>();
            if (cookieUser == null) throw new CoronaDL.Exceptions.NotLoggedInException();
            var match = _dbUser.SelectByMail(cookieUser.LoginMail);
            if (match == null || match.Hash != cookieUser.Hash) throw new CoronaDL.Exceptions.InvalidHashException();
            if (match.HashValidUntil < DateTime.Now) throw new CoronaDL.Exceptions.NotLoggedInException(); // Expired
            return match;
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
                ValidationCode = CreateRandomHash()
               
            };
            _dbUser.Insert(newUser);
            _mail.Validate(newUser);
        }
    }
}

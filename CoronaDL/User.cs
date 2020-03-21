using CoronaDL.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;

namespace CoronaDL
{
    public class User : IUser
    {
        private DataConnection _connection = new DataConnection();

        public IEnumerable<CoronaEntities.User> GetAll()
        {
            return _connection.SelectAll<CoronaEntities.User>();
        }

        public void Insert(CoronaEntities.User user)
        {
            _connection.Insert(user);
        }

        public CoronaEntities.User SelectByMail(string mail)
        {
            return GetAll().FirstOrDefault(q => q.LoginMail == mail);
        }

        private CoronaEntities.User SelectById(Guid id)
        {
            var usr = GetAll().FirstOrDefault(q => q.Id == id);
            return usr;
        }

        public void Update(CoronaEntities.User user)
        {
            _connection.Update(user);
        }
    }
}
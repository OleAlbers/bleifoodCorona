using CoronaDL.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;

namespace CoronaDL
{
    public class User : IUser
    {
        

        public IEnumerable<CoronaEntities.User> GetAll()
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                return connection.SelectAll<CoronaEntities.User>().ToList();
            }
        }

        public void Insert(CoronaEntities.User user)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Insert(user);
            }
        }

        public CoronaEntities.User SelectByMail(string mail)
        {
            return GetAll().FirstOrDefault(q => q.Credentials.LoginMail == mail);
        }

        private CoronaEntities.User SelectById(Guid id)
        {
            var usr = GetAll().FirstOrDefault(q => q.Id == id);
            return usr;
        }

        public void Update(CoronaEntities.User user)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Update(user);

            }
        }
    }
}
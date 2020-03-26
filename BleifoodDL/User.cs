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

        public void Update(CoronaEntities.User user)
        {
            using (var database = DataConnection.GetDatabase())
            {
                var connection = new DataConnection(database);
                connection.Update(user);

            }
        }

        public CoronaEntities.User SelectByHash(string hash)
        {
            return GetAll().FirstOrDefault(q => q.Credentials.Hash == hash); ;
        }
    }
}
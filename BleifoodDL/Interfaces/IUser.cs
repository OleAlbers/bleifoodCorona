using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CoronaDL.Interfaces
{
    public interface IUser
    {
        void Insert(CoronaEntities.User user);
        void Update(CoronaEntities.User user);
        CoronaEntities.User SelectByMail(string mail);
        CoronaEntities.User SelectByHash(string hash);
        IEnumerable<CoronaEntities.User> GetAll();
    }
}

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
        IEnumerable<CoronaEntities.User> GetAll();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaBL
{
    public interface IUser
    {
        void Login(string mail, string password);
        void Register(string mail, string password);
        void Activate(string mail, string hash);
        void ChangePassword(string mail, string passwordOld, string passwordnew);
    }
}

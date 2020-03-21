namespace CoronaBL.Interfaces
{
    public interface IUser
    {
        void Login(string mail, string password);
        void Register(string mail, string password);
        void Activate(string mail, string hash);
        void ChangePassword(string mail, string passwordOld, string passwordnew);
        CoronaEntities.User GetFromCookie();
    }
}
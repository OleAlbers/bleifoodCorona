using System;

namespace CoronaBL.Interfaces
{
    public interface ICookies
    {
        DateTime SetCookie<T>(T data, TimeSpan expiration, bool httpOnly = true, bool secure = true);

        T GetCookie<T>() where T : new();
    }
}
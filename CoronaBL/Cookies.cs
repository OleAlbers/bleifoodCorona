using CoronaEntities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CoronaBL
{
    public static class Cookies
    {
        private const string Prefix = "Bleifood_";

        public static DateTime SetCookie<T>(T data, TimeSpan expiration, bool httpOnly = true, bool secure = true)
        {
            Type dataType = typeof(T);

            HttpCookie userCookie = new HttpCookie(Prefix + dataType.Name);
            userCookie.HttpOnly = httpOnly;
            userCookie.Secure = secure;

            if (data != null)
            {
                foreach (var property in dataType.GetProperties())
                {
                    var storeInCookieAttribute = property.GetCustomAttribute<StoreInCookieAttribute>();
                    if (storeInCookieAttribute == null) continue;
                    userCookie[property.Name] = property.GetValue(data) as string;
                }
            }

            userCookie.Expires.Add(expiration);
            HttpContext.Current.Response.Cookies.Add(userCookie);
            return DateTime.Now.Add(expiration);
        }

        public static T GetCookie<T>() where T : new()
        {
            Type dataType = typeof(T);

            var cookie = HttpContext.Current.Request.Cookies[Prefix + dataType.Name];
            if (cookie == null) return default(T);
            var result = new T();
            foreach (var property in dataType.GetProperties())
            {
                
                var storeInCookieAttribute = property.GetCustomAttribute<StoreInCookieAttribute>();
                if (storeInCookieAttribute == null) continue;
                property.SetValue(result, cookie[property.Name]);
            }
            
            return result;
        }
    }
}

using CoronaBL.Interfaces;
using CoronaEntities.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CoronaBL
{
    public class Cookies:ICookies
    {
        private IHttpContextAccessor _httpContextAccessor;
        public Cookies(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        private const string Prefix = "Bleifood";

        public  DateTime SetCookie<T>(T data, TimeSpan expiration, bool httpOnly = true, bool secure = true)
        {
            DateTime expireDate = DateTime.Now.Add(expiration);
            if (data == null) return DateTime.Now;
            Type dataType = typeof(T);
            var response = _httpContextAccessor.HttpContext.Response;
            CookieOptions cookieOptions = new CookieOptions
            {
                HttpOnly = httpOnly,
                Secure = secure,
                Expires = expireDate
            };

            foreach (var property in dataType.GetProperties())
            {
                var storeInCookieAttribute = property.GetCustomAttribute<StoreInCookieAttribute>();
                if (storeInCookieAttribute == null) continue;
                response.Cookies.Append(BuildCookieKey(dataType.Name, property.Name), property.GetValue(data) as string, cookieOptions);
            }

            return expireDate;
        }

        private string BuildCookieKey(string datatypeName, string propertyName)
        {
            return $"{Prefix}.{datatypeName}.{propertyName}";
        }

        public T GetCookie<T>() where T : new()
        {
            Type dataType = typeof(T);
            var request = _httpContextAccessor.HttpContext.Request;
            var result = new T();

            foreach (var property in dataType.GetProperties())
            {
                var storeInCookieAttribute = property.GetCustomAttribute<StoreInCookieAttribute>();
                if (storeInCookieAttribute == null) continue;
                var cookieName = BuildCookieKey(dataType.Name, property.Name);
                if (request.Cookies.ContainsKey(cookieName))
                {
                    property.SetValue(result, request.Cookies[cookieName]);
                }
            }
            
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebUtils.Utils;
using WebUtils.Web;

namespace WebUtils.Frmwk.Web
{
    internal class ResponseCookiesAccessor : IAccessor<ICookie>
    {
        private readonly HttpCookieCollection cookies;

        public ResponseCookiesAccessor(HttpCookieCollection cookies)
        {
            this.cookies = cookies;
        }

        public int Count 
        {
            get { 
                return cookies.Count;
            }
        }

        public bool Contains(string key)
        {
            return cookies[key] != null;
        }

        public IEnumerable<ICookie> Get(Expression<Func<ICookie, bool>> where)
        {
           throw new NotSupportedException("Filtering cookies is not supported in Response");
        }

        public ICookie Get(string key)
        {
            return cookies[key] != null 
                ? new CookieWrapper(cookies[key]) 
                : null;
        }

        public IEnumerable<string> GetKeys()
        {
            return cookies.AllKeys;
        }

        public void Set(string key, ICookie value)
        {
            cookies.Set(new HttpCookie(key)
            {
                Value = value.Value,
                Domain = value.Domain,
                Path = value.Path,
                Expires = value.Expires == DateTimeOffset.MinValue ? DateTime.MinValue : value.Expires.UtcDateTime,
                Secure = value.Secure,
                HttpOnly = value.HttpOnly
            });
        }
    }
}
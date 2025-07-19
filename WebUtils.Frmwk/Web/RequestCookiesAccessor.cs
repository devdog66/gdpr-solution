using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebUtils.Utils;
using WebUtils.Web;

namespace WebUtils.Frmwk.Web
{
    internal class RequestCookiesAccessor : IAccessor<ICookie>
    {
        private readonly HttpCookieCollection cookies;

        public RequestCookiesAccessor(HttpCookieCollection cookies)
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
            throw new NotImplementedException("Filtering cookies is not supported in Request");
        }

        public ICookie Get(string key)
        {
            var cookie = cookies[key];
            if(cookie == null)
                return null;
            else
                return new CookieWrapper(cookie);
        }

        public IEnumerable<string> GetKeys()
        {
            return cookies.AllKeys;
        }

        public void Set(string key, ICookie value)
        {
            throw new NotImplementedException("Setting cookies is not supported in Request");
        }
    }
}
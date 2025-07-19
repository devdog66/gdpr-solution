using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using WebUtils.Utils;
using WebUtils.Web;

namespace WebUtils.Core.Web
{
    public class ResponseCookiesAccessor(IResponseCookies cookies, IAccessor<string> headers) : IAccessor<ICookie>
    {
        private readonly IResponseCookies cookies = cookies;
        private readonly IAccessor<string> headers = headers;

        public int Count
        {
            get 
            {
                // The IResponseCookies interface does not provide a Count property,
                // so we need to use the underlying collection to get the count.
                return GetKeys().Count();
            }
        }

        public IEnumerable<ICookie> Get(Expression<Func<ICookie, bool>> where)
        {
            throw new NotImplementedException();
        }

        public ICookie Get(string key)
        {
            throw new NotImplementedException();    
        }

        public bool Contains(string key)
        {
            return GetKeys().Contains(key);
        }

        public void Set(string key, ICookie value)
        {
            cookies.Append(key, value.Value, new CookieOptions
            {
                Expires = value.Expires,
                HttpOnly = value.HttpOnly,
                Secure = value.Secure,
                SameSite = (Microsoft.AspNetCore.Http.SameSiteMode)value.SameSite
            });
        }

        //TODO: Needs work for cookies with expiration and domain
        public IEnumerable<string> GetKeys()
        {
            var cookies = headers.Get(t => t.Equals("Set-Cookie"));

            List<string> list = [];
            foreach (var cookie in cookies)
            {
                string text = cookie;
                string[] array = text.Split([',']);
                string[] array2 = array;
                foreach (string text2 in array2)
                {
                    string[] array3 = text2.Split(['=']);
                    list.Add(array3[0]);
                }
            }

            return list;
        }
    }
}
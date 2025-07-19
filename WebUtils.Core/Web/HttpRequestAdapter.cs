using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using System.Net;
using WebUtils.Utils;
using WebUtils.Web;

namespace WebUtils.Core.Web
{
    public class HttpRequestAdapter(HttpRequest httpRequest) : IHttpRequestFacade
    {
        private readonly HttpRequest request = httpRequest;

        public string UserAgent
        {
            get
            {
                return Headers.Get("User-Agent");
            }
        }

        public IAccessor<string> Headers
        {
            get
            {
                return new HeadersAccessor(request.Headers);
            }
        }

        public string Host
        {
            get
            {
                return request.Host.Value;
            }
        }

        public string Method
        {
            get
            {
                return request.Method;
            }
        }

        public string Path
        {
            get
            {
                return request.Path.Value;
            }
        }

        public IAccessor<ICookie> Cookies
        {
            get
            {
                return new RequestCookieAccessor(request.Cookies);
            }
        }

        public string Referer
        {
            get
            {
                return Headers.Get("Referer");
            }
        }
    }

    internal class RequestCookieAccessor(IRequestCookieCollection cookies) : IAccessor<ICookie>
    {
        private readonly IRequestCookieCollection cookies = cookies;

        public int Count
        {             
            get
            {
                return cookies.Count;
            }
        }

        public bool Contains(string key)
        {
            return cookies.ContainsKey(key);
        }

        public IEnumerable<ICookie> Get(Expression<Func<ICookie, bool>> where)
        {
            throw new NotImplementedException();
        }

        public ICookie? Get(string key)
        {
            if (Contains(key))
            {
                var cookie = new Cookie
                {
                    Name = key,
                    Value = cookies[key]
                };
                return new CookieImpl
                {
                    Name = key,
                    Value = cookie.Value
                };
            }
            return null;
        }

        public IEnumerable<string> GetKeys()
        {
            var retVal = new List<string>();

            foreach (var key in cookies.Keys)
            {
                retVal.Add(key.ToString());
            }

            return retVal;
        }

        public void Set(string key, ICookie value)
        {
            throw new NotImplementedException();
        }
    }

    public class CookieImpl : ICookie
    {
        public string? Domain { get; set; }
        public string? Path { get; set; }
        public DateTimeOffset Expires { get; set; }
        public bool Secure { get; set; }
        public WebUtils.Web.SameSiteEnum SameSite { get; set; }
        public bool HttpOnly { get; set; }
        public TimeSpan? MaxAge { get; set; }
        public bool IsEssential { get; set; }
        public string? Value { get; set; }
        public string? Name { get; set; }
    }
}

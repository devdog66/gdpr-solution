using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using WebUtils.Utils;

namespace WebUtils.Frmwk.Web
{
    internal class HeadersAccessor : IAccessor<string>
    {
        private readonly NameValueCollection headers;

        public HeadersAccessor(NameValueCollection headers)
        {
            this.headers = headers;
        }

        public int Count
        {
            get {
                return headers?.Count ?? 0;
            }
        }

        public bool Contains(string key)
        {
            return headers != null && headers[key] != null;
        }

        public IEnumerable<string> Get(Expression<Func<string, bool>> where)
        {
            return headers?.AllKeys
                .Where(key => where.Compile()(headers[key])) ?? Enumerable.Empty<string>();
        }

        public string Get(string key)
        {
            return headers?[key];
        }

        public IEnumerable<string> GetKeys()
        {
            return headers?.AllKeys ?? Enumerable.Empty<string>();
        }

        public void Set(string key, string value)
        {
            headers[key] = value ?? throw new ArgumentNullException(nameof(value), "Value cannot be null.");
        }
    }
}
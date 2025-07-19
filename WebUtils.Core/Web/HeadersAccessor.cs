using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Linq.Expressions;
using WebUtils.Utils;

namespace WebUtils.Core.Web
{
    public class HeadersAccessor(IHeaderDictionary headers) : IAccessor<string>
    {
        private readonly IHeaderDictionary headers = headers;

        public int Count
        {
            get
            {
                return headers.Count;
            }
        }

        public bool Contains(string key)
        {
            return headers.ContainsKey(key);
        }

        public IEnumerable<string> Get(Expression<Func<string, bool>> where)
        {
            var retVal = new List<string>();
            var keys = headers.Keys.Where(where.Compile());
            foreach (var key in keys)
            {
                var value = headers[key];
                if (!StringValues.IsNullOrEmpty(value)) // Ensure value is not null or empty
                {
                    retVal.Add(value.ToString()); // Convert to string explicitly
                }
            }
            return retVal;
        }

        public string Get(string key)
        {
            return headers.TryGetValue(key, out var value) ? value.ToString() : string.Empty;
        }

        public IEnumerable<string> GetKeys()
        {
            var retVal = new List<string>();

            foreach (var key in headers.Keys)
            {
                retVal.Add(key.ToString());
            }

            return retVal;
        }

        public void Set(string key, string value)
        {
            headers.Add(key, value);
        }
    }
}
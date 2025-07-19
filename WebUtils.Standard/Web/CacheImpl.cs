using Microsoft.Extensions.Caching.Memory;
using System;
using WebUtils.Web;

namespace WebUtils.Standard.Web
{
    public class CacheImpl : ICacheFacade
    {
        private readonly IMemoryCache _cache;

        public CacheImpl(IMemoryCache cache) 
        {
            _cache = cache;
        }

        public void Set<T>(string key, T value, DateTimeOffset? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
        {
            var entry = _cache.CreateEntry(key);
            entry.Value = value;
            entry.AbsoluteExpiration = absoluteExpiration;
            entry.SlidingExpiration = slidingExpiration;

        }

        public bool TryGetValue<T>(string key, out T value)
        {
            return _cache.TryGetValue(key, out value);
        }
    }
}

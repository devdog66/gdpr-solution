using System;

namespace WebUtils.Web
{
    public interface ICacheFacade
    {
        bool TryGetValue<T>(string key, out T value);
        void Set<T>(string key, T value, DateTimeOffset? absoluteExpiration = null, TimeSpan? slidingExpiration = null);
    }
}

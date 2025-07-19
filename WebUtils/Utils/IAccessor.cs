using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WebUtils.Utils
{
    public interface IAccessor<T>
    {
        int Count { get; }
        IEnumerable<T> Get(Expression<Func<T, bool>> where);
        T Get(string key);
        bool Contains(string key);
        void Set(string key, T value);
        IEnumerable<string> GetKeys();
    }
}
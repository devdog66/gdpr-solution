using System;
using System.Linq;
using System.Linq.Expressions;

namespace WebUtils.Data
{
    public interface IRepository : IDisposable
    {
        T Create<T>(T entity) where T : class;
        IQueryable<T> Read<T>(Expression<Func<T, bool>> where) where T : class;
        int Update<T>(T entity) where T : class;
        int Delete<T>(T entity) where T : class;
    }
}

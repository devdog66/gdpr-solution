using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Linq.Expressions;
using WebUtils.Data;
using WebUtils.Security;

namespace WebUtils.Domain.Repositories
{
    public class BaseRepository : IRepository
    {
        private readonly DbContext ctx;

        public BaseRepository(IConfiguration configuration, IVault vault)
        {
            var connectionString = configuration.GetValue<string>("WebUtilsDb");

            var connStrBuilder = new MySqlConnectionStringBuilder(connectionString!)
            {
                Password = vault.GetSecret("WebUtilsDbPassword")
            };
            var connStr = connStrBuilder.ToString();
            ctx = new WebUtilsDbContext(connStr);
        }

        public void Dispose()
        {
            ctx.Dispose();
            GC.SuppressFinalize(this);
        }

        public T Create<T>(T entity) where T : class
        {
            var entityEntry = ctx.Set<T>().Add(entity);
            entityEntry.State = EntityState.Added;
            ctx.SaveChanges();
            return entityEntry.Entity;
        }

        public IQueryable<T> Read<T>(Expression<Func<T, bool>> where) where T : class
        {
            var queryable = ctx.Set<T>().Where(where);
            return queryable;
        }

        public int Update<T>(T entity) where T : class
        {
            var entry = ctx.Entry(entity);
            entry.State = EntityState.Modified;
            var rowsAffected = ctx.SaveChanges();
            return rowsAffected;
        }

        public int Delete<T>(T entity) where T : class
        {
            var entry = ctx.Entry(entity);
            entry.State = EntityState.Deleted;
            var rowsAffected = ctx.SaveChanges();
            return rowsAffected;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using WebUtils.Domain.Models;

namespace WebUtils.Domain.Repositories
{
    internal class WebUtilsDbContext : DbContext
    {
        private readonly string connectionString;

        internal WebUtilsDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
            optionsBuilder.UseMySql(connectionString, serverVersion);
        }

        internal DbSet<Request> Requests { get; set; }
        internal DbSet<Consent> Consents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}

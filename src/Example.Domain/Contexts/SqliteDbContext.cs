using Insql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Domain.Contexts
{
    public class SqliteDbContext<T> : DbContext where T : class
    {
        public SqliteDbContext(DbContextOptions<SqliteDbContext<T>> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptions options)
        {
            var configuration = options.ServiceProvider.GetRequiredService<IConfiguration>();

            options.UseSqlResolver<T>();

            options.UserSqlite(configuration.GetConnectionString("sqlite"));
        }
    }
}

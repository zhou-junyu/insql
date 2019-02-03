using Insql.Sqlite;
using System.Data;

namespace Insql
{
    public static partial class DbContextOptionsExtensions
    {
        public static DbContextOptions UseSqlite(this DbContextOptions options)
        {
            options.ConnectionFactory = SqliteDbConnectionFactory.Instance;

            return options;
        }

        public static DbContextOptions UseSqlite(this DbContextOptions options, string connectionString)
        {
            options.ConnectionFactory = SqliteDbConnectionFactory.Instance;
            options.ConnectionString = connectionString;

            return options;
        }

        public static DbContextOptions UseSqlite(this DbContextOptions options, IDbConnection connection)
        {
            options.ConnectionFactory = SqliteDbConnectionFactory.Instance;
            options.ConnectionString = connection.ConnectionString;
            options.Connection = connection;

            return options;
        }
    }
}

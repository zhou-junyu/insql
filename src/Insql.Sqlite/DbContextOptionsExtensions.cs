using Insql.Sqlite;
using System.Data;

namespace Insql
{
    public static partial class DbContextOptionsExtensions
    {
        public static DbContextOptions UseSqlite(this DbContextOptions options)
        {
            options.ConnectionFactory = SqliteDbConnectionFactory.Instance;

            options.SqlResolverEnvironment["DbType"] = "Sqlite";

            return options;
        }

        public static DbContextOptions UseSqlite(this DbContextOptions options, string connectionString)
        {
            options.ConnectionFactory = SqliteDbConnectionFactory.Instance;
            options.ConnectionString = connectionString;

            options.SqlResolverEnvironment["DbType"] = "Sqlite";

            return options;
        }

        public static DbContextOptions UseSqlite(this DbContextOptions options, IDbConnection connection)
        {
            options.ConnectionFactory = SqliteDbConnectionFactory.Instance;
            options.ConnectionString = connection.ConnectionString;
            options.Connection = connection;

            options.SqlResolverEnvironment["DbType"] = "Sqlite";

            return options;
        }
    }
}

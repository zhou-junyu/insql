using Insql.Sqlite;
using System.Data;

namespace Insql
{
    public static partial class DbContextOptionsExtensions
    {
        public static DbContextOptions UseSqlite(this DbContextOptions options, string connectionString)
        {
            options.SessionFactory = new SqliteDbSessionFactory(options, connectionString);

            options.SqlResolveEnv["DbType"] = "Sqlite";

            return options;
        }

        public static DbContextOptions UseSqlite(this DbContextOptions options, IDbConnection connection)
        {
            options.SessionFactory = new SqliteDbSessionFactory(options, connection);

            options.SqlResolveEnv["DbType"] = "Sqlite";

            return options;
        }
    }
}

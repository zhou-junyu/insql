using Insql.PostgreSql;
using System.Data;

namespace Insql
{
    public static partial class DbContextOptionsExtensions
    {
        public static DbContextOptions UsePostgreSql(this DbContextOptions options, string connectionString)
        {
            options.SessionFactory = new PostgreSqlDbSessionFactory(options, connectionString);

            options.SqlResolverEnvironment["DbType"] = "PostgreSql";

            return options;
        }

        public static DbContextOptions UsePostgreSql(this DbContextOptions options, IDbConnection connection)
        {
            options.SessionFactory = new PostgreSqlDbSessionFactory(options, connection);

            options.SqlResolverEnvironment["DbType"] = "PostgreSql";

            return options;
        }
    }
}

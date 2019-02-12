using Insql.PostgreSql;
using System.Data;

namespace Insql
{
    public static partial class DbContextOptionsExtensions
    {
        public static DbContextOptions UsePostgreSql(this DbContextOptions options)
        {
            options.ConnectionFactory = PostgreSqlDbConnectionFactory.Instance;

            options.ServerName = "PostgreSql";

            return options;
        }

        public static DbContextOptions UsePostgreSql(this DbContextOptions options, string connectionString)
        {
            options.ConnectionFactory = PostgreSqlDbConnectionFactory.Instance;
            options.ConnectionString = connectionString;

            options.ServerName = "PostgreSql";

            return options;
        }

        public static DbContextOptions UsePostgreSql(this DbContextOptions options, IDbConnection connection)
        {
            options.ConnectionFactory = PostgreSqlDbConnectionFactory.Instance;
            options.ConnectionString = connection.ConnectionString;
            options.Connection = connection;

            options.ServerName = "PostgreSql";

            return options;
        }
    }
}

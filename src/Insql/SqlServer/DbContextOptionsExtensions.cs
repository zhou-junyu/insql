using Insql.SqlServer;
using System.Data;

namespace Insql
{
    public static partial class DbContextOptionsExtensions
    {
        public static DbContextOptions UseSqlServer(this DbContextOptions options)
        {
            options.ConnectionFactory = SqlServerDbConnectionFactory.Instance;

            options.SqlResolverEnvironment["DbType"] = "SqlServer";

            return options;
        }

        public static DbContextOptions UseSqlServer(this DbContextOptions options, string connectionString)
        {
            options.ConnectionFactory = SqlServerDbConnectionFactory.Instance;
            options.ConnectionString = connectionString;

            options.SqlResolverEnvironment["DbType"] = "SqlServer";

            return options;
        }

        public static DbContextOptions UseSqlServer(this DbContextOptions options, IDbConnection connection)
        {
            options.ConnectionFactory = SqlServerDbConnectionFactory.Instance;
            options.ConnectionString = connection.ConnectionString;
            options.Connection = connection;

            options.SqlResolverEnvironment["DbType"] = "SqlServer";

            return options;
        }
    }
}

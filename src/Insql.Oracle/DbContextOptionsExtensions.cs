using Insql.Oracle;
using System.Data;

namespace Insql
{
    public static partial class DbContextOptionsExtensions
    {
        public static DbContextOptions UseOracle(this DbContextOptions options)
        {
            options.ConnectionFactory = OracleDbConnectionFactory.Instance;

            options.SqlResolverEnvironment["DbType"] = "Oracle";

            return options;
        }

        public static DbContextOptions UseOracle(this DbContextOptions options, string connectionString)
        {
            options.ConnectionFactory = OracleDbConnectionFactory.Instance;
            options.ConnectionString = connectionString;

            options.SqlResolverEnvironment["DbType"] = "Oracle";

            return options;
        }

        public static DbContextOptions UseOracle(this DbContextOptions options, IDbConnection connection)
        {
            options.ConnectionFactory = OracleDbConnectionFactory.Instance;
            options.ConnectionString = connection.ConnectionString;
            options.Connection = connection;

            options.SqlResolverEnvironment["DbType"] = "Oracle";

            return options;
        }
    }
}

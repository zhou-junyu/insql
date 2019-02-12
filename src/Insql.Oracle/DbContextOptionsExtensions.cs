using Insql.Oracle;
using System.Data;

namespace Insql
{
    public static partial class DbContextOptionsExtensions
    {
        public static DbContextOptions UseOracle(this DbContextOptions options)
        {
            options.ConnectionFactory = OracleDbConnectionFactory.Instance;

            options.ServerName = "Oracle";

            return options;
        }

        public static DbContextOptions UseOracle(this DbContextOptions options, string connectionString)
        {
            options.ConnectionFactory = OracleDbConnectionFactory.Instance;
            options.ConnectionString = connectionString;

            options.ServerName = "Oracle";

            return options;
        }

        public static DbContextOptions UseOracle(this DbContextOptions options, IDbConnection connection)
        {
            options.ConnectionFactory = OracleDbConnectionFactory.Instance;
            options.ConnectionString = connection.ConnectionString;
            options.Connection = connection;

            options.ServerName = "Oracle";

            return options;
        }
    }
}

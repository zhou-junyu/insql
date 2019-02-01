using Insql.Oracle;
using System.Data;

namespace Insql
{
    public static partial class DbContextOptionsExtensions
    {
        public static DbContextOptions UserOracle(this DbContextOptions options)
        {
            options.ConnectionFactory = OracleDbConnectionFactory.Instance;

            return options;
        }

        public static DbContextOptions UserOracle(this DbContextOptions options, string connectionString)
        {
            options.ConnectionFactory = OracleDbConnectionFactory.Instance;
            options.ConnectionString = connectionString;

            return options;
        }

        public static DbContextOptions UserOracle(this DbContextOptions options, IDbConnection connection)
        {
            options.ConnectionFactory = OracleDbConnectionFactory.Instance;
            options.ConnectionString = connection.ConnectionString;
            options.Connection = connection;

            return options;
        }
    }
}

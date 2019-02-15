using Insql.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Insql
{
    public static partial class DbContextOptionsExtensions
    {
        public static DbContextOptions UseOracle(this DbContextOptions options, string connectionString)
        {
            options.SessionFactory = new OracleDbSessionFactory(options, connectionString);

            options.SqlResolverEnvironment["DbType"] = "Oracle";

            return options;
        }

        public static DbContextOptions UseOracle(this DbContextOptions options, string connectionString, OracleCredential connectionCredential)
        {
            options.SessionFactory = new OracleDbSessionFactory(options, connectionString, connectionCredential);

            options.SqlResolverEnvironment["DbType"] = "Oracle";

            return options;
        }

        public static DbContextOptions UseOracle(this DbContextOptions options, IDbConnection connection)
        {
            options.SessionFactory = new OracleDbSessionFactory(options, connection);

            options.SqlResolverEnvironment["DbType"] = "Oracle";

            return options;
        }
    }
}

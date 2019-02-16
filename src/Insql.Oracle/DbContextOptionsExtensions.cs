using Insql.Resolvers;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace Insql
{
    public static partial class DbContextOptionsExtensions
    {
        public static DbContextOptions UseOracle(this DbContextOptions options, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            options.SqlResolveEnviron.SetDbType("Oracle");

            options.DbSession = new DbSession(new OracleConnection(connectionString), true);

            return options;
        }

        public static DbContextOptions UseOracle(this DbContextOptions options, IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            options.SqlResolveEnviron.SetDbType("Oracle");

            options.DbSession = new DbSession(connection, false);

            return options;
        }
    }
}

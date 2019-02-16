using Npgsql;
using System;
using System.Data;

namespace Insql
{
    public static partial class DbContextOptionsExtensions
    {
        public static DbContextOptions UsePostgreSql(this DbContextOptions options, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            options.SqlResolveEnviron.SetDbType("PostgreSql");

            options.DbSession = new DbSession(new NpgsqlConnection(connectionString), true);

            return options;
        }

        public static DbContextOptions UsePostgreSql(this DbContextOptions options, IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            options.SqlResolveEnviron.SetDbType("PostgreSql");

            options.DbSession = new DbSession(connection, false);

            return options;
        }
    }
}

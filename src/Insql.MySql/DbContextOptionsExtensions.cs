using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Insql
{
    public static partial class DbContextOptionsExtensions
    {
        public static DbContextOptions UseMySql(this DbContextOptions options, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            options.SqlResolveEnviron.SetDbType("MySql");

            options.DbSession = new DbSession(new MySqlConnection(connectionString), true);

            return options;
        }

        public static DbContextOptions UseMySql(this DbContextOptions options, IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            options.SqlResolveEnviron.SetDbType("MySql");

            options.DbSession = new DbSession(connection, false);

            return options;
        }
    }
}

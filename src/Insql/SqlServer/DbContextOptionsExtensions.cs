using System;
using System.Data;
using System.Data.SqlClient;

namespace Insql
{
    public static partial class DbContextOptionsExtensions
    {
        public static DbContextOptions UseSqlServer(this DbContextOptions options, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            options.SqlResolveEnviron.SetDbType("SqlServer");

            options.DbSession = new DbSession(new SqlConnection(connectionString), true);

            return options;
        }

        public static DbContextOptions UseSqlServer(this DbContextOptions options, IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            options.SqlResolveEnviron.SetDbType("SqlServer");

            options.DbSession = new DbSession(connection, false);

            return options;
        }
    }
}

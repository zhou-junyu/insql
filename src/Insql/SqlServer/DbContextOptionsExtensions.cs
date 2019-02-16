using System;
using System.Data;
using System.Data.SqlClient;

namespace Insql
{
    public static partial class DbContextOptionsExtensions
    {
        public static DbContextOptions UseSqlServer(this DbContextOptions options, string connectionString)
        {
            return options.UseSqlServer(connectionString, null);
        }

        public static DbContextOptions UseSqlServer(this DbContextOptions options, string connectionString, Action<DbSessionOptions> configure)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            var sessionOptions = new DbSessionOptions();

            configure?.Invoke(sessionOptions);

            options.SqlResolveEnv.SetDbType("SqlServer");

            options.DbSession = new DbSession(new SqlConnection(connectionString), true)
            {
                CommandTimeout = sessionOptions.CommandTimeout
            };

            return options;
        }

        public static DbContextOptions UseSqlServer(this DbContextOptions options, IDbConnection connection)
        {
            return options.UseSqlServer(connection, null);
        }

        public static DbContextOptions UseSqlServer(this DbContextOptions options, IDbConnection connection, Action<DbSessionOptions> configure)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            var sessionOptions = new DbSessionOptions();

            configure?.Invoke(sessionOptions);

            options.SqlResolveEnv.SetDbType("SqlServer");

            options.DbSession = new DbSession(connection, false)
            {
                CommandTimeout = sessionOptions.CommandTimeout
            };

            return options;
        }
    }
}

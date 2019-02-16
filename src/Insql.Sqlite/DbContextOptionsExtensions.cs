using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace Insql
{
    public static partial class DbContextOptionsExtensions
    {
        public static DbContextOptions UseSqlite(this DbContextOptions options, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            options.SqlResolveEnviron.SetDbType("Sqlite");

            options.DbSession = new DbSession(new SqliteConnection(connectionString), true);

            return options;
        }

        public static DbContextOptions UseSqlite(this DbContextOptions options, IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            options.SqlResolveEnviron.SetDbType("Sqlite");

            options.DbSession = new DbSession(connection, false);

            return options;
        }
    }
}

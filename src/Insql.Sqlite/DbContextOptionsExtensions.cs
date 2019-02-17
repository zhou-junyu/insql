using Insql.Sqlite;
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

            options.ResolveEnviron.SetDbType("Sqlite");

            options.SessionFactory = new SqliteDbSessionFactory(options, connectionString);

            return options;
        }

        public static DbContextOptions UseSqlite(this DbContextOptions options, IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            options.ResolveEnviron.SetDbType("Sqlite");

            options.SessionFactory = new SqliteDbSessionFactory(options, connection);

            return options;
        }
    }
}

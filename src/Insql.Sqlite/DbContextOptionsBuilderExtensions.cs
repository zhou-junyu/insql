using Insql.Sqlite;
using System;
using System.Data;

namespace Insql
{
    public static partial class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseSqlite(this DbContextOptionsBuilder builder, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            builder.Options.Dialect = SqliteDialect.Instance;
            builder.Options.SessionFactory = new SqliteSessionFactory(builder.Options, connectionString);

            return builder;
        }

        public static DbContextOptionsBuilder UseSqlite(this DbContextOptionsBuilder builder, IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            builder.Options.Dialect = SqliteDialect.Instance;
            builder.Options.SessionFactory = new SqliteSessionFactory(builder.Options, connection);

            return builder;
        }
    }
}

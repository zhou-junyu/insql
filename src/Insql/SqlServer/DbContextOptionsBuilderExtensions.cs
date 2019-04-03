using Insql.SqlServer;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Insql
{
    public static partial class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseSqlServer(this DbContextOptionsBuilder builder, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            builder.Options.Dialect = SqlServerDialect.Instance;
            builder.Options.SessionFactory = new SqlServerSessionFactory(builder.Options, connectionString);

            return builder;
        }

        public static DbContextOptionsBuilder UseSqlServer(this DbContextOptionsBuilder builder, string connectionString, SqlCredential connectionCredential)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            builder.Options.Dialect = SqlServerDialect.Instance;
            builder.Options.SessionFactory = new SqlServerSessionFactory(builder.Options, connectionString, connectionCredential);

            return builder;
        }

        public static DbContextOptionsBuilder UseSqlServer(this DbContextOptionsBuilder builder, IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            builder.Options.Dialect = SqlServerDialect.Instance;
            builder.Options.SessionFactory = new SqlServerSessionFactory(builder.Options, connection);

            return builder;
        }
    }
}

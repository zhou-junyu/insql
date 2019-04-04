using Insql.PostgreSql;
using System;
using System.Data;

namespace Insql
{
    public static partial class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UsePostgreSql(this DbContextOptionsBuilder builder, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            builder.Options.Dialect = PostgreSqlDialect.Instance;
            builder.Options.SessionFactory = new PostgreSqlSessionFactory(builder.Options, connectionString);

            return builder;
        }

        public static DbContextOptionsBuilder UsePostgreSql(this DbContextOptionsBuilder builder, IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            builder.Options.Dialect = PostgreSqlDialect.Instance;
            builder.Options.SessionFactory = new PostgreSqlSessionFactory(builder.Options, connection);

            return builder;
        }
    }
}

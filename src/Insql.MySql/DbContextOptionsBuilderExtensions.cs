using Insql.MySql;
using System;
using System.Data;

namespace Insql
{
    public static partial class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseMySql(this DbContextOptionsBuilder builder, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            builder.Options.Dialect = MySqlDialect.Instance;
            builder.Options.SessionFactory = new MySqlSessionFactory(builder.Options, connectionString);

            return builder;
        }

        public static DbContextOptionsBuilder UseMySql(this DbContextOptionsBuilder builder, IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            builder.Options.Dialect = MySqlDialect.Instance;
            builder.Options.SessionFactory = new MySqlSessionFactory(builder.Options, connection);

            return builder;
        }
    }
}

using Insql.SqlServer;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Insql
{
    public static partial class DbContextOptionsExtensions
    {
        public static DbContextOptionsBuilder UseSqlServer(this DbContextOptionsBuilder builder, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            builder.Options.SessionFactory = new SqlServerSessionFactory(builder, connectionString);

            return builder;
        }

        public static DbContextOptions UseSqlServer(this DbContextOptions options, string connectionString, SqlCredential credential)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            options.SessionFactory = new SqlServerSessionFactory(options, connectionString, credential);

            return options;
        }

        public static DbContextOptions UseSqlServer(this DbContextOptions options, IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            options.SessionFactory = new SqlServerSessionFactory(options, connection);

            return options;
        }
    }
}

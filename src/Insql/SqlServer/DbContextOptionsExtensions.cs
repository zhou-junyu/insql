using Insql.SqlServer;
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

            options.SessionFactory = new SqlServerDbSessionFactory(options, connectionString);

            return options;
        }

        public static DbContextOptions UseSqlServer(this DbContextOptions options, string connectionString, SqlCredential credential)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            options.SessionFactory = new SqlServerDbSessionFactory(options, connectionString, credential);

            return options;
        }

        public static DbContextOptions UseSqlServer(this DbContextOptions options, IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            options.SessionFactory = new SqlServerDbSessionFactory(options, connection);

            return options;
        }
    }
}

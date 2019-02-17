using Insql.MySql;
using System;
using System.Data;

namespace Insql
{
    public static partial class DbContextOptionsExtensions
    {
        public static DbContextOptions UseMySql(this DbContextOptions options, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            options.ResolveEnviron.SetDbType("MySql");

            options.SessionFactory = new MySqlDbSessionFactory(options, connectionString);

            return options;
        }

        public static DbContextOptions UseMySql(this DbContextOptions options, IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            options.ResolveEnviron.SetDbType("MySql");

            options.SessionFactory = new MySqlDbSessionFactory(options, connection);

            return options;
        }
    }
}

using Insql.SqlServer;
using System.Data;

namespace Insql
{
    public static partial class DbContextOptionsExtensions
    {
        public static DbContextOptions UseSqlServer(this DbContextOptions options, string connectionString)
        {
            options.SessionFactory = new SqlServerDbSessionFactory(options, connectionString);

            options.SqlResolverEnvironment["DbType"] = "SqlServer";

            return options;
        }

        public static DbContextOptions UseSqlServer(this DbContextOptions options, IDbConnection connection)
        {
            options.SessionFactory = new SqlServerDbSessionFactory(options, connection);

            options.SqlResolverEnvironment["DbType"] = "SqlServer";

            return options;
        }
    }
}

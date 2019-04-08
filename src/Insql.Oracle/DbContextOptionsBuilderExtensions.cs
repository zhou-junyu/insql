using Insql.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace Insql
{
    public static partial class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseOracle(this DbContextOptionsBuilder builder, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            builder.Options.Dialect = OracleDialect.Instance;
            builder.Options.SessionFactory = new OracleSessionFactory(builder.Options, connectionString);

            return builder;
        }

        public static DbContextOptionsBuilder UseOracle(this DbContextOptionsBuilder builder, string connectionString, OracleCredential connectionCredential)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            builder.Options.Dialect = OracleDialect.Instance;
            builder.Options.SessionFactory = new OracleSessionFactory(builder.Options, connectionString, connectionCredential);

            return builder;
        }

        public static DbContextOptionsBuilder UseOracle(this DbContextOptionsBuilder builder, IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            builder.Options.Dialect = OracleDialect.Instance;
            builder.Options.SessionFactory = new OracleSessionFactory(builder.Options, connection);

            return builder;
        }
    }
}

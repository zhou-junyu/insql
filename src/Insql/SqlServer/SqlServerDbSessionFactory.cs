using System;
using System.Data;
using System.Data.SqlClient;

namespace Insql.SqlServer
{
    public class SqlServerDbSessionFactory : IDbSessionFactory
    {
        private readonly DbContextOptions contextOptions;
        private readonly IDbConnection dbConnection;
        private readonly string connectionString;
        private readonly SqlCredential credential;

        public SqlServerDbSessionFactory(DbContextOptions contextOptions, IDbConnection dbConnection)
        {
            if (contextOptions == null)
            {
                throw new ArgumentNullException(nameof(contextOptions));
            }
            if (dbConnection == null)
            {
                throw new ArgumentNullException(nameof(dbConnection));
            }

            this.contextOptions = contextOptions;
            this.dbConnection = dbConnection;
        }

        public SqlServerDbSessionFactory(DbContextOptions contextOptions, string connectionString)
        {
            if (contextOptions == null)
            {
                throw new ArgumentNullException(nameof(contextOptions));
            }
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            this.contextOptions = contextOptions;
            this.connectionString = connectionString;
        }

        public SqlServerDbSessionFactory(DbContextOptions contextOptions, string connectionString, SqlCredential credential)
        {
            if (contextOptions == null)
            {
                throw new ArgumentNullException(nameof(contextOptions));
            }
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            this.contextOptions = contextOptions;
            this.connectionString = connectionString;
            this.credential = credential;
        }


        public IDbSession CreateSession()
        {
            if (this.dbConnection != null)
            {
                return new DbSession(this.dbConnection, false)
                {
                    CommandTimeout = this.contextOptions.CommandTimeout,
                    SupportMultipleStatements = true
                };
            }

            if (this.credential != null)
            {
                return new DbSession(new SqlConnection(connectionString, credential), true)
                {
                    CommandTimeout = this.contextOptions.CommandTimeout,
                    SupportMultipleStatements = true
                };
            }

            return new DbSession(new SqlConnection(connectionString), true)
            {
                CommandTimeout = this.contextOptions.CommandTimeout,
                SupportMultipleStatements = true
            };
        }
    }
}

using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace Insql.Oracle
{
    public class OracleDbSessionFactory : IDbSessionFactory
    {
        private readonly DbContextOptions contextOptions;
        private readonly IDbConnection dbConnection;
        private readonly string connectionString;
        private readonly OracleCredential credential;

        public OracleDbSessionFactory(DbContextOptions contextOptions, IDbConnection dbConnection)
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

        public OracleDbSessionFactory(DbContextOptions contextOptions, string connectionString)
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

        public OracleDbSessionFactory(DbContextOptions contextOptions, string connectionString, OracleCredential credential)
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
                    SupportMultipleStatements = false
                };
            }

            if (this.credential != null)
            {
                return new DbSession(new OracleConnection(connectionString, credential), true)
                {
                    CommandTimeout = this.contextOptions.CommandTimeout,
                    SupportMultipleStatements = false
                };
            }

            return new DbSession(new OracleConnection(connectionString), true)
            {
                CommandTimeout = this.contextOptions.CommandTimeout,
                SupportMultipleStatements = false
            };
        }
    }
}

using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Insql.MySql
{
    internal class MySqlDbSessionFactory : IDbSessionFactory
    {
        private readonly DbContextOptions contextOptions;
        private readonly IDbConnection dbConnection;
        private readonly string connectionString;

        public MySqlDbSessionFactory(DbContextOptions contextOptions, IDbConnection dbConnection)
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

        public MySqlDbSessionFactory(DbContextOptions contextOptions, string connectionString)
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

        public IDbSession CreateSession()
        {
            if (this.dbConnection != null)
            {
                return new DbSession(this.dbConnection, false)
                {
                    DbType = "MySql",
                    SupportStatements = true,
                    CommandTimeout = this.contextOptions.CommandTimeout,
                };
            }

            return new DbSession(new MySqlConnection(connectionString), true)
            {
                DbType = "MySql",
                SupportStatements = true,
                CommandTimeout = this.contextOptions.CommandTimeout,
            };
        }
    }
}

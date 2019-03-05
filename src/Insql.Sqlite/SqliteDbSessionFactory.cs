//using Microsoft.Data.Sqlite;
//using System;
//using System.Data;

//namespace Insql.Sqlite
//{
//    internal class SqliteDbSessionFactory : IDbSessionFactory
//    {
//        private readonly DbContextOptions contextOptions;
//        private readonly IDbConnection dbConnection;
//        private readonly string connectionString;

//        public SqliteDbSessionFactory(DbContextOptions contextOptions, IDbConnection dbConnection)
//        {
//            if (contextOptions == null)
//            {
//                throw new ArgumentNullException(nameof(contextOptions));
//            }
//            if (dbConnection == null)
//            {
//                throw new ArgumentNullException(nameof(dbConnection));
//            }

//            this.contextOptions = contextOptions;
//            this.dbConnection = dbConnection;
//        }

//        public SqliteDbSessionFactory(DbContextOptions contextOptions, string connectionString)
//        {
//            if (contextOptions == null)
//            {
//                throw new ArgumentNullException(nameof(contextOptions));
//            }
//            if (string.IsNullOrWhiteSpace(connectionString))
//            {
//                throw new ArgumentNullException(nameof(connectionString));
//            }

//            this.contextOptions = contextOptions;
//            this.connectionString = connectionString;
//        }

//        public IDbSession CreateSession()
//        {
//            if (this.dbConnection != null)
//            {
//                return new DbSession(this.dbConnection, false)
//                {
//                    DbType = "Sqlite",
//                    SupportStatements = true,
//                    CommandTimeout = this.contextOptions.CommandTimeout,
//                };
//            }

//            return new DbSession(new SqliteConnection(connectionString), true)
//            {
//                DbType = "Sqlite",
//                SupportStatements = true,
//                CommandTimeout = this.contextOptions.CommandTimeout,
//            };
//        }
//    }
//}

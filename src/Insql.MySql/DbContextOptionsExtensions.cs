//using Insql.MySql;
//using System;
//using System.Data;

//namespace Insql
//{
//    public static partial class DbContextOptionsExtensions
//    {
//        public static DbContextOptions UseMySql(this DbContextOptions options, string connectionString)
//        {
//            if (string.IsNullOrWhiteSpace(connectionString))
//            {
//                throw new ArgumentNullException(nameof(connectionString));
//            }

//            options.SqlAdapter = new MySqlAdapter(options, connectionString);

//            return options;
//        }

//        public static DbContextOptions UseMySql(this DbContextOptions options, IDbConnection connection)
//        {
//            if (connection == null)
//            {
//                throw new ArgumentNullException(nameof(connection));
//            }

//            options.SqlAdapter = new MySqlAdapter(options, connection);

//            return options;
//        }
//    }
//}

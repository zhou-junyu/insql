using Microsoft.Data.Sqlite;
using System.Data;

namespace Insql.Sqlite
{
    internal class SqliteDbConnectionFactory : IDbConnectionFactory
    {
        public static SqliteDbConnectionFactory Instance = new SqliteDbConnectionFactory();

        public IDbConnection CreateConnection()
        {
            return new SqliteConnection();
        }
    }
}

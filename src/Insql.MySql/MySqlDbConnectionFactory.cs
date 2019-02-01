using MySql.Data.MySqlClient;
using System.Data;

namespace Insql.MySql
{
    internal class MySqlDbConnectionFactory : IDbConnectionFactory
    {
        public static MySqlDbConnectionFactory Instance = new MySqlDbConnectionFactory();

        public IDbConnection CreateConnection()
        {
            return new MySqlConnection();
        }
    }
}

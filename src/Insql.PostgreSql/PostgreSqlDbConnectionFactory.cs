using Npgsql;
using System.Data;

namespace Insql.PostgreSql
{
    internal class PostgreSqlDbConnectionFactory : IDbConnectionFactory
    {
        public static PostgreSqlDbConnectionFactory Instance = new PostgreSqlDbConnectionFactory();

        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection();
        }
    }
}

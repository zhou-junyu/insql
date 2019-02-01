using System.Data;
using System.Data.SqlClient;

namespace Insql.SqlServer
{
    internal class SqlServerDbConnectionFactory : IDbConnectionFactory
    {
        public static SqlServerDbConnectionFactory Instance = new SqlServerDbConnectionFactory();

        public IDbConnection CreateConnection()
        {
            return new SqlConnection();
        }
    }
}

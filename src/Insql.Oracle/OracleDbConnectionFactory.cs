using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Insql.Oracle
{
    internal class OracleDbConnectionFactory : IDbConnectionFactory
    {
        public static OracleDbConnectionFactory Instance = new OracleDbConnectionFactory();

        public IDbConnection CreateConnection()
        {
            return new OracleConnection();
        }
    }
}

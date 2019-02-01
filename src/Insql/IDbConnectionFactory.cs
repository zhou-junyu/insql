using System.Data;

namespace Insql
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}

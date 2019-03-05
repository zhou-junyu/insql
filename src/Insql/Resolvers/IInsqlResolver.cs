using System.Collections.Generic;

namespace Insql.Resolvers
{
    public interface IInsqlResolver
    {
        ResolveResult Resolve(string dbType, string sqlId, IDictionary<string, object> sqlParam);
    }
}

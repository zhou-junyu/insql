using System.Collections.Generic;

namespace Insql.Resolvers
{
    public interface IInsqlResolver
    {
        ResolveResult Resolve(string sqlId, IDictionary<string, object> sqlParam);
    }
}

using System.Collections.Generic;

namespace Insql.Resolvers
{
    public interface ISqlResolveFilter
    {
        void OnResolving(InsqlDescriptor insqlDescriptor, string dbType, string sqlId, IDictionary<string, object> sqlParam);

        void OnResolved(ResolveContext resolveContext, ResolveResult resolveResult);
    }
}

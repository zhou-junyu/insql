using System.Collections.Generic;

namespace Insql.Resolvers
{
    public interface ISqlResolveFilter
    {
        void OnResolving(InsqlDescriptor insqlDescriptor, ResolveEnviron resolveEnviron, string sqlId, IDictionary<string, object> sqlParam);

        void OnResolved(InsqlDescriptor insqlDescriptor, ResolveContext resolveContext, ResolveResult resolveResult);
    }
}

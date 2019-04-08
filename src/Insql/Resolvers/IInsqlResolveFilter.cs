using System.Collections.Generic;

namespace Insql.Resolvers
{
    public interface IInsqlResolveFilter
    {
        void OnResolving(InsqlDescriptor insqlDescriptor, string sqlId, IDictionary<string, object> sqlParam);

        void OnResolved(ResolveContext resolveContext, ResolveResult resolveResult);
    }
}

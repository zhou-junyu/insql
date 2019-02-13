using System.Collections.Generic;

namespace Insql.Resolvers
{
    public interface ISqlResolveFilter
    {
        void OnResolving(InsqlDescriptor insqlDescriptor, string sqlId, IDictionary<string, object> sqlParam, IDictionary<string, string> envParam);

        void OnResolved(InsqlDescriptor insqlDescriptor, ResolveContext resolveContext, ResolveResult resolveResult);
    }
}

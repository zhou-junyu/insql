using System.Collections.Generic;

namespace Insql.Resolvers
{
    public interface ISqlResolveFilter
    {
        void OnPre(InsqlDescriptor insqlDescriptor, string sqlId, IDictionary<string, object> sqlParam, IDictionary<string, string> envParam);

        void OnPost(InsqlDescriptor insqlDescriptor, ResolveContext resolveContext, ResolveResult resolveResult);
    }
}

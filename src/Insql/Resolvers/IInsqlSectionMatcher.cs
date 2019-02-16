using System.Collections.Generic;

namespace Insql.Resolvers
{
    public interface IInsqlSectionMatcher
    {
        IInsqlSection Match(InsqlDescriptor insqlDescriptor, ResolveEnviron resolveEnviron, string sqlId, IDictionary<string, object> sqlParam);
    }
}

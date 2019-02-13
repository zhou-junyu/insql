using System.Collections.Generic;

namespace Insql.Resolvers
{
    public interface IInsqlSectionMatcher
    {
        IInsqlSection Match(InsqlDescriptor insqlDescriptor, string sqlId, IDictionary<string, object> sqlParam, IDictionary<string, string> envParam);
    }
}

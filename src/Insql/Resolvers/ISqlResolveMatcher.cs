using System.Collections.Generic;

namespace Insql.Resolvers
{
    public interface ISqlResolveMatcher
    {
        IInsqlSection Match(InsqlDescriptor insqlDescriptor, string dbType, string sqlId, IDictionary<string, object> sqlParam);
    }
}

using System.Collections.Generic;

namespace Insql.Resolvers
{
    public interface ISqlResolver
    {
        /// <summary>
        /// resolve sql.
        /// </summary>
        /// <param name="resolveEnviron"></param>
        /// <param name="sqlId"></param>
        /// <param name="sqlParam"></param>
        /// <returns></returns>
        ResolveResult Resolve(ResolveEnviron resolveEnviron, string sqlId, IDictionary<string, object> sqlParam);
    }
}

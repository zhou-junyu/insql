using System.Collections.Generic;

namespace Insql.Resolvers
{
    public interface ISqlResolver
    {
        /// <summary>
        /// resolve sql.
        /// </summary>
        /// <param name="sqlId"></param>
        /// <param name="sqlParam"></param>
        /// <param name="envParam"></param>
        /// <returns></returns>
        ResolveResult Resolve(string sqlId, IDictionary<string, object> sqlParam, IDictionary<string, string> envParam);
    }
}

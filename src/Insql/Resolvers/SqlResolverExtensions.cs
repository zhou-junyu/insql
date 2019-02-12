using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Insql.Resolvers
{
    public static partial class SqlResolverExtensions
    {
        /// <summary>
        /// resolve sql.
        /// </summary>
        /// <param name="sqlResolver"></param>
        /// <param name="sqlId"></param>
        /// <param name="sqlParam">support plain object or idictionary</param>
        /// <returns></returns>
        public static ResolveResult Resolve(this ISqlResolver sqlResolver, string sqlId, object sqlParam)
        {
            if (sqlParam == null)
            {
                return sqlResolver.Resolve(sqlId, (IDictionary<string, object>)null);
            }

            var dictionaryParam = sqlParam as IEnumerable<KeyValuePair<string, object>>;

            if (dictionaryParam == null)
            {
                dictionaryParam = sqlParam.GetType()
               .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
               .Select(propInfo => new KeyValuePair<string, object>(propInfo.Name, propInfo.GetValue(sqlParam, null)));
            }

            return sqlResolver.Resolve(sqlId, dictionaryParam.ToDictionary(item => item.Key, item => item.Value));
        }
    }
}

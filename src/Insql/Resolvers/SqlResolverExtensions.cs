using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Insql.Resolvers
{
    public static partial class SqlResolverExtensions
    {
        /// <summary>
        ///  resolve sql.
        /// </summary>
        /// <param name="sqlResolver"></param>
        /// <param name="sqlId"></param>
        /// <param name="sqlParam"></param>
        /// <returns></returns>
        public static ResolveResult Resolve(this ISqlResolver sqlResolver, string sqlId, IDictionary<string, object> sqlParam)
        {
            return sqlResolver.Resolve(sqlId, sqlParam, (IDictionary<string, string>)null);
        }

        /// <summary>
        /// resolve sql.
        /// </summary>
        /// <param name="sqlResolver"></param>
        /// <param name="sqlId"></param>
        /// <param name="sqlParam">support plain object or idictionary</param>
        /// <returns></returns>
        public static ResolveResult Resolve(this ISqlResolver sqlResolver, string sqlId, object sqlParam)
        {
            return sqlResolver.Resolve(sqlId, sqlParam, (IDictionary<string, string>)null);
        }

        /// <summary>
        /// resolve sql.
        /// </summary>
        /// <param name="sqlResolver"></param>
        /// <param name="sqlId"></param>
        /// <param name="sqlParam"></param>
        /// <param name="envParam"></param>
        /// <returns></returns>
        public static ResolveResult Resolve(this ISqlResolver sqlResolver, string sqlId, object sqlParam, IDictionary<string, string> envParam)
        {
            if (sqlParam == null)
            {
                return sqlResolver.Resolve(sqlId, (IDictionary<string, object>)null, envParam);
            }

            var sqlParamDictionary = sqlParam as IEnumerable<KeyValuePair<string, object>>;

            if (sqlParamDictionary == null)
            {
                sqlParamDictionary = sqlParam.GetType()
               .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
               .Select(propInfo => new KeyValuePair<string, object>(propInfo.Name, propInfo.GetValue(sqlParam, null)));
            }

            return sqlResolver.Resolve(sqlId, sqlParamDictionary.ToDictionary(item => item.Key, item => item.Value), envParam);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Insql.Resolvers
{
    public static partial class SqlResolverExtensions
    {
        public static ResolveResult Resolve(this ISqlResolver sqlResolver, string sqlId)
        {
            return sqlResolver.Resolve((string)null, sqlId, (IDictionary<string, object>)null);
        }

        public static ResolveResult Resolve(this ISqlResolver sqlResolver, string sqlId, IDictionary<string, object> sqlParam)
        {
            return sqlResolver.Resolve((string)null, sqlId, sqlParam);
        }

        public static ResolveResult Resolve(this ISqlResolver sqlResolver, string sqlId, object sqlParam)
        {
            return sqlResolver.Resolve((string)null, sqlId, sqlParam);
        }

        public static ResolveResult Resolve(this ISqlResolver sqlResolver, string dbType, string sqlId, object sqlParam)
        {
            if (sqlParam == null)
            {
                return sqlResolver.Resolve(dbType, sqlId, (IDictionary<string, object>)null);
            }

            var sqlParamDictionary = sqlParam as IEnumerable<KeyValuePair<string, object>>;

            if (sqlParamDictionary == null)
            {
                sqlParamDictionary = sqlParam.GetType()
               .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
               .Select(propInfo => new KeyValuePair<string, object>(propInfo.Name, propInfo.GetValue(sqlParam, null)));
            }

            return sqlResolver.Resolve(dbType, sqlId, sqlParamDictionary.ToDictionary(item => item.Key, item => item.Value));
        }
    }
}

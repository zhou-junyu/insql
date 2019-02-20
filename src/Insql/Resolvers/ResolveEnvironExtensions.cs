using Insql.Resolvers;
using System;

namespace Insql
{
    public static partial class ResolveEnvironExtensions
    {
        [Obsolete("will be removed in the new version")]
        public static ResolveEnviron SetDbType(this ResolveEnviron resolveEnv, string dbType)
        {
            return resolveEnv.Set("DbType", dbType);
        }

        [Obsolete("will be removed in the new version")]
        public static string GetDbType(this ResolveEnviron resolveEnv)
        {
            return resolveEnv.Get("DbType");
        }
    }
}

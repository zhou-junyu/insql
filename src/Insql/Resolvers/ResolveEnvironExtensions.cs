using Insql.Resolvers;

namespace Insql
{
    public static partial class ResolveEnvironExtensions
    {
        public static ResolveEnviron SetDbType(this ResolveEnviron resolveEnv, string dbType)
        {
            return resolveEnv.Set("DbType", dbType);
        }

        public static string GetDbType(this ResolveEnviron resolveEnv)
        {
            return resolveEnv.Get("DbType");
        }
    }
}

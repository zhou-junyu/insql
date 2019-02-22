using System;
using System.Collections.Generic;

namespace Insql.Resolvers
{
    internal class SqlResolver<T> : ISqlResolver<T> where T : class
    {
        private readonly ISqlResolver resolver;

        public SqlResolver(ISqlResolverFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            this.resolver = factory.GetResolver(typeof(T));
        }

        public ResolveResult Resolve(string dbType, string sqlId, IDictionary<string, object> sqlParam)
        {
            return this.resolver.Resolve(dbType, sqlId, sqlParam);
        }
    }
}

using System;
using System.Collections.Generic;

namespace Insql.Resolvers
{
    public class SqlResolver<T> : ISqlResolver<T> where T : class
    {
        private readonly ISqlResolver _resolver;

        public SqlResolver(ISqlResolverFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            this._resolver = factory.GetResolver(typeof(T));
        }

        public ResolveResult Resolve(string sqlId, IDictionary<string, object> sqlParam)
        {
            return this._resolver.Resolve(sqlId, sqlParam);
        }
    }
}

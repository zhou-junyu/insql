using System;
using System.Collections.Generic;

namespace Insql.Resolvers
{
    internal class InsqlResolver<T> : IInsqlResolver<T> where T : class
    {
        private readonly IInsqlResolver resolver;

        public InsqlResolver(IInsqlResolverFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            this.resolver = factory.CreateResolver(typeof(T));
        }
        
        public ResolveResult Resolve(string dbType, string sqlId, IDictionary<string, object> sqlParam)
        {
            return this.resolver.Resolve(dbType, sqlId, sqlParam);
        }
    }
}

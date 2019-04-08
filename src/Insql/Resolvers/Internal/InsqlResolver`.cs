using System;
using System.Collections.Generic;

namespace Insql.Resolvers
{
    internal class InsqlResolver<TContext> : IInsqlResolver<TContext> where TContext : class
    {
        private readonly IInsqlResolver resolver;

        public InsqlResolver(IInsqlResolverFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            this.resolver = factory.CreateResolver(typeof(TContext));
        }

        public ResolveResult Resolve(string sqlId, IDictionary<string, object> sqlParam)
        {
            return this.resolver.Resolve(sqlId, sqlParam);
        }
    }
}

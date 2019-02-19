using Insql.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Insql.Resolvers
{
    public class SqlResolverFactory : ISqlResolverFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly Dictionary<Type, InsqlDescriptor> insqlDescriptors = new Dictionary<Type, InsqlDescriptor>();

        public SqlResolverFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            foreach (var provider in serviceProvider.GetServices<IInsqlDescriptorProvider>())
            {
                foreach (var descriptor in provider.GetDescriptors())
                {
                    insqlDescriptors[descriptor.Type] = descriptor;
                }
            }
        }

        public ISqlResolver GetResolver(Type type)
        {
            if (insqlDescriptors.TryGetValue(type, out InsqlDescriptor insqlDescriptor))
            {
                return new SqlResolver(insqlDescriptor, this.serviceProvider);
            }

            return null;
        }
    }
}

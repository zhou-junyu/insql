using Insql.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Insql.Resolvers
{
    public class SqlResolverFactory : ISqlResolverFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ISqlResolveMatcher resolveMatcher;
        private readonly IEnumerable<ISqlResolveFilter> resolveFilters;
        private readonly IEnumerable<IInsqlDescriptorProvider> descriptorProviders;

        private readonly Dictionary<Type, InsqlDescriptor> insqlDescriptors = new Dictionary<Type, InsqlDescriptor>();

        public SqlResolverFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            this.resolveMatcher = serviceProvider.GetRequiredService<ISqlResolveMatcher>();
            this.resolveFilters = serviceProvider.GetServices<ISqlResolveFilter>();
            this.descriptorProviders = serviceProvider.GetServices<IInsqlDescriptorProvider>();

            foreach (var provider in this.descriptorProviders)
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
                return new SqlResolver(insqlDescriptor, this.serviceProvider, this.resolveMatcher, this.resolveFilters);
            }

            throw new Exception($"InsqlDescriptor : `{type.FullName}` not found !");
        }
    }
}

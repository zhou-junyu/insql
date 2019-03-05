using Insql.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Insql.Resolvers
{
    internal class InsqlResolverFactory : IInsqlResolverFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IInsqlDescriptorLoader descriptorLoader;
        private readonly IInsqlResolveMatcher resolveMatcher;
        private readonly IEnumerable<IInsqlResolveFilter> resolveFilters;
        private readonly IDictionary<Type, InsqlDescriptor> insqlDescriptors;
        private readonly ConcurrentDictionary<Type, InsqlDescriptor> defaultDescriptors;

        public InsqlResolverFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            this.descriptorLoader = serviceProvider.GetRequiredService<IInsqlDescriptorLoader>();
            this.resolveMatcher = serviceProvider.GetRequiredService<IInsqlResolveMatcher>();
            this.resolveFilters = serviceProvider.GetServices<IInsqlResolveFilter>();

            this.insqlDescriptors = this.descriptorLoader.Load();
            this.defaultDescriptors = new ConcurrentDictionary<Type, InsqlDescriptor>();
        }

        public IInsqlResolver CreateResolver(Type scopeType)
        {
            if (scopeType == null)
            {
                throw new ArgumentNullException(nameof(scopeType));
            }

            if (!this.insqlDescriptors.TryGetValue(scopeType, out InsqlDescriptor insqlDescriptor))
            {
                insqlDescriptor = this.defaultDescriptors.GetOrAdd(scopeType, (stype) => new InsqlDescriptor(stype));
            }

            return new InsqlResolver(insqlDescriptor, this.serviceProvider, this.resolveMatcher, this.resolveFilters);
        }
    }
}

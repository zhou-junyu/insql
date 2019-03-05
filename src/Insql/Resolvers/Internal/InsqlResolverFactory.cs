using Insql.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
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

        public InsqlResolverFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            this.descriptorLoader = serviceProvider.GetRequiredService<IInsqlDescriptorLoader>();
            this.resolveMatcher = serviceProvider.GetRequiredService<IInsqlResolveMatcher>();
            this.resolveFilters = serviceProvider.GetServices<IInsqlResolveFilter>();

            this.insqlDescriptors = this.descriptorLoader.Load();
        }

        public IInsqlResolver GetResolver(Type type)
        {
            if (insqlDescriptors.TryGetValue(type, out InsqlDescriptor insqlDescriptor))
            {
                return new InsqlResolver(insqlDescriptor, this.serviceProvider, this.resolveMatcher, this.resolveFilters);
            }

            //todo need return default empty resolver

            throw new Exception($"InsqlDescriptor : `{type.FullName}` not found !");
        }
    }
}

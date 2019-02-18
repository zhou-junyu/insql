using Insql.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;

namespace Insql.Resolvers
{
    public class SqlResolverFactory : ISqlResolverFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ConcurrentDictionary<Type, InsqlDescriptor> insqlDescriptors;

        public SqlResolverFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.insqlDescriptors = new ConcurrentDictionary<Type, InsqlDescriptor>();

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

            foreach (var provider in serviceProvider.GetServices<IInsqlDescriptorProvider>())
            {
                foreach (var descriptor in provider.GetDescriptors())
                {
                    if (!this.insqlDescriptors.ContainsKey(descriptor.Type))
                    {
                        this.insqlDescriptors.TryAdd(descriptor.Type, descriptor);
                    }
                }
            }

            if (insqlDescriptors.TryGetValue(type, out insqlDescriptor))
            {
                return new SqlResolver(insqlDescriptor, this.serviceProvider);
            }

            throw new Exception($"InsqlDescriptor : {type.FullName} not found !");
        }
    }
}

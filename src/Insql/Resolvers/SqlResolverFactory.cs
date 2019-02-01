using Insql.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Insql.Resolvers
{
    public class SqlResolverFactory : ISqlResolverFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly Dictionary<Type, InsqlDescriptor> descriptors = new Dictionary<Type, InsqlDescriptor>();

        public SqlResolverFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            foreach (var provider in serviceProvider.GetServices<IInsqlDescriptorProvider>())
            {
                foreach (var descriptor in provider.GetDescriptors())
                {
                    descriptors[descriptor.Type] = descriptor;
                }
            }
        }

        public ISqlResolver GetResolver(Type type)
        {
            if (descriptors.TryGetValue(type, out InsqlDescriptor descriptor))
            {
                return new SqlResolver(descriptor, this.serviceProvider);
            }

            throw new Exception($"InsqlDescriptor : {type.FullName} not found !");
        }
    }
}

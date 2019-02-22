using Insql.Mappers;
using Insql.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Insql.Resolvers
{
    internal class SqlResolverFactory : ISqlResolverFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ISqlResolveMatcher resolveMatcher;
        private readonly IInsqlDescriptorMapper descriptorMapper;
        private readonly IEnumerable<ISqlResolveFilter> resolveFilters;
        private readonly IEnumerable<IInsqlDescriptorProvider> descriptorProviders;

        private readonly Dictionary<Type, InsqlDescriptor> insqlDescriptors = new Dictionary<Type, InsqlDescriptor>();

        public SqlResolverFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            this.resolveMatcher = serviceProvider.GetRequiredService<ISqlResolveMatcher>();
            this.descriptorMapper = serviceProvider.GetRequiredService<IInsqlDescriptorMapper>();
            this.resolveFilters = serviceProvider.GetServices<ISqlResolveFilter>();
            this.descriptorProviders = serviceProvider.GetServices<IInsqlDescriptorProvider>();

            this.LoadInsqlDescriptors();

            this.descriptorMapper.Mapping(this.insqlDescriptors.Values);
        }

        public ISqlResolver GetResolver(Type type)
        {
            if (insqlDescriptors.TryGetValue(type, out InsqlDescriptor insqlDescriptor))
            {
                return new SqlResolver(insqlDescriptor, this.serviceProvider, this.resolveMatcher, this.resolveFilters);
            }

            throw new Exception($"InsqlDescriptor : `{type.FullName}` not found !");
        }

        private void LoadInsqlDescriptors()
        {
            foreach (var provider in this.descriptorProviders)
            {
                foreach (var descriptor in provider.GetDescriptors())
                {
                    if (this.insqlDescriptors.TryGetValue(descriptor.Type, out InsqlDescriptor insqlDescriptor))
                    {
                        //sections
                        foreach (var section in descriptor.Sections)
                        {
                            insqlDescriptor.Sections[section.Key] = section.Value;
                        }
                        //maps
                        foreach (var map in descriptor.Maps)
                        {
                            insqlDescriptor.Maps[map.Key] = map.Value;
                        }
                    }
                    else
                    {
                        this.insqlDescriptors.Add(descriptor.Type, descriptor);
                    }
                }
            }
        }
    }
}

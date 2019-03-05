using Insql.Resolvers;
using System;
using System.Collections.Generic;

namespace Insql.Providers
{
    internal class InsqlDescriptorLoader : IInsqlDescriptorLoader
    {
        private readonly IEnumerable<IInsqlDescriptorProvider> descriptorProviders;

        private readonly Lazy<Dictionary<Type, InsqlDescriptor>> insqlDescriptors;

        public InsqlDescriptorLoader(IEnumerable<IInsqlDescriptorProvider> descriptorProviders)
        {
            this.descriptorProviders = descriptorProviders;

            this.insqlDescriptors = new Lazy<Dictionary<Type, InsqlDescriptor>>(this.LoadInsqlDescriptors, true);
        }

        public IDictionary<Type, InsqlDescriptor> Load()
        {
            return this.insqlDescriptors.Value;
        }

        private Dictionary<Type, InsqlDescriptor> LoadInsqlDescriptors()
        {
            var resultDescriptors = new Dictionary<Type, InsqlDescriptor>();

            foreach (var provider in this.descriptorProviders)
            {
                foreach (var descriptor in provider.GetDescriptors())
                {
                    if (resultDescriptors.TryGetValue(descriptor.Type, out InsqlDescriptor insqlDescriptor))
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
                        resultDescriptors.Add(descriptor.Type, descriptor);
                    }
                }
            }

            return resultDescriptors;
        }
    }
}

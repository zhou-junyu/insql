using Insql.Resolvers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Insql.Providers.Embedded
{
    public class EmbeddedDescriptorProvider : IInsqlDescriptorProvider
    {
        private readonly IOptions<EmbeddedDescriptorOptions> options;

        public EmbeddedDescriptorProvider(IOptions<EmbeddedDescriptorOptions> options)
        {
            this.options = options;
        }

        public IEnumerable<InsqlDescriptor> GetDescriptors()
        {
            var optionsValue = this.options.Value;

            if (string.IsNullOrWhiteSpace(optionsValue.Locations))
            {
                return new List<InsqlDescriptor>();
            }

            GlobHelper glob = new GlobHelper(optionsValue.Locations);

            IEnumerable<Assembly> assemblies = optionsValue.Assemblies;

            if (assemblies == null || assemblies.Count() < 1)
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies();
            }

            assemblies = assemblies.Where(assembly => !assembly.IsDynamic && !assembly.ReflectionOnly);

            return assemblies.SelectMany(assembly =>
            {
                var resourceNames = assembly.GetManifestResourceNames();

                resourceNames = glob.Filter(resourceNames).ToArray();

                return resourceNames
                .Select(name => InsqlDescriptorXmlParser.Instance.ParseDescriptor(assembly.GetManifestResourceStream(name), optionsValue.Namespace))
                .Where(o => o != null);
            }).ToList();
        }
    }
}

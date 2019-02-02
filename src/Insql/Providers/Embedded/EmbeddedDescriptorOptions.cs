using System.Collections.Generic;
using System.Reflection;

namespace Insql.Providers.Embedded
{
    public class EmbeddedDescriptorOptions
    {
        public List<Assembly> Assemblies { get; set; }

        public string Locations { get; set; } = "**/*.insql.xml";

        public string Namespace { get; set; } = "";
    }
}

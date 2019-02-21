using System.Collections.Generic;
using System.Reflection;

namespace Insql.Providers.EmbeddedXml
{
    public class EmbeddedDescriptorOptions
    {
        public List<Assembly> Assemblies { get; set; }

        public string Matches { get; set; }

        public string Namespace { get; set; }

        public bool Enabled { get; set; }
    }
}

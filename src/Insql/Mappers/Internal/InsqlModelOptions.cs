using System.Collections.Generic;
using System.Reflection;

namespace Insql.Mappers
{
    internal class InsqlModelOptions
    {
        public bool AnnotationMapScanEnabled { get; set; }

        public List<Assembly> AnnotationMapScanAssemblies { get; set; }

        public bool FluentMapScanEnabled { get; set; }

        public List<Assembly> FluentMapScanAssemblies { get; set; }

        public bool XmlMapEnabled { get; set; }
    }
}

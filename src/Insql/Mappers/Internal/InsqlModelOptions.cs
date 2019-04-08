using System.Collections.Generic;
using System.Reflection;

namespace Insql.Mappers
{
    internal class InsqlModelOptions
    {
        public bool IncludeAnnotationMaps { get; set; }

        public List<Assembly> IncludeAnnotationMapsAssemblies { get; set; }

        public bool IncludeFluentMaps { get; set; }

        public List<Assembly> IncludeFluentMapsAssemblies { get; set; }

        public bool ExcludeXmlMaps { get; set; }
    }
}

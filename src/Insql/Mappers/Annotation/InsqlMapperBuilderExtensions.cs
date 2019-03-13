using Insql.Mappers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Insql
{
    public static partial class InsqlMapperBuilderExtensions
    {
        public static IInsqlMapperBuilder EnabledAnnotationMapScan(this IInsqlMapperBuilder builder)
        {
            builder.Services.Configure<InsqlModelOptions>(options =>
            {
                options.AnnotationMapScanEnabled = true;
            });

            return builder;
        }

        public static IInsqlMapperBuilder EnabledAnnotationMapScan(this IInsqlMapperBuilder builder, IEnumerable<Assembly> assemblies)
        {
            builder.Services.Configure<InsqlModelOptions>(options =>
            {
                options.AnnotationMapScanEnabled = true;
                options.AnnotationMapScanAssemblies = assemblies.ToList();
            });

            return builder;
        }
    }
}

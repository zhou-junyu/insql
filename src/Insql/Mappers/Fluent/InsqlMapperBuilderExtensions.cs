using Insql.Mappers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Insql
{
    public static partial class InsqlMapperBuilderExtensions
    {
        public static IInsqlMapperBuilder EnabledFluentMapScan(this IInsqlMapperBuilder builder)
        {
            builder.Services.Configure<InsqlModelOptions>(options =>
            {
                options.FluentMapScanEnabled = true;
            });

            return builder;
        }

        public static IInsqlMapperBuilder EnabledFluentMapScan(this IInsqlMapperBuilder builder, IEnumerable<Assembly> assemblies)
        {
            builder.Services.Configure<InsqlModelOptions>(options =>
            {
                options.FluentMapScanEnabled = true;
                options.FluentMapScanAssemblies = assemblies.ToList();
            });

            return builder;
        }
    }
}

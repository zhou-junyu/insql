using Insql.Mappers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Insql
{
    public static partial class InsqlMapperBuilderExtensions
    {
        public static InsqlMapperBuilder IncludeFluentMaps(this InsqlMapperBuilder builder)
        {
            builder.Services.Configure<InsqlModelOptions>(options =>
            {
                options.IncludeFluentMaps = true;
            });

            return builder;
        }

        public static InsqlMapperBuilder IncludeFluentMaps(this InsqlMapperBuilder builder, IEnumerable<Assembly> assemblies)
        {
            builder.Services.Configure<InsqlModelOptions>(options =>
            {
                options.IncludeFluentMaps = true;
                options.IncludeFluentMapsAssemblies = assemblies.ToList();
            });

            return builder;
        }
    }
}

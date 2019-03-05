using Insql.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace Insql
{
    public static partial class InsqlMapperBuilderExtensions
    {
        public static IInsqlMapperBuilder AddAnnotationMap(this IInsqlMapperBuilder builder)
        {
            builder.Services.Configure<InsqlModelOptions>(options =>
            {
                options.AnnotationMapEnabled = true;
            });

            return builder;
        }
    }
}

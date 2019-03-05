using Insql.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace Insql
{
    public static partial class InsqlMapperBuilderExtensions
    {
        public static IInsqlMapperBuilder AddFluentMap(this IInsqlMapperBuilder builder)
        {
            builder.Services.Configure<InsqlModelOptions>(options =>
            {
                options.FluentMapEnabled = true;
            });

            return builder;
        }
    }
}

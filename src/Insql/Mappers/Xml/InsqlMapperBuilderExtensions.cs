using Insql.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace Insql
{
    public static partial class InsqlMapperBuilderExtensions
    {
        public static InsqlMapperBuilder ExcludeXmlMaps(this InsqlMapperBuilder builder)
        {
            builder.Services.Configure<InsqlModelOptions>(options =>
            {
                options.ExcludeXmlMaps = false;
            });

            return builder;
        }
    }
}

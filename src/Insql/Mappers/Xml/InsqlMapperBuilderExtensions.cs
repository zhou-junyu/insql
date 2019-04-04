using Insql.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace Insql
{
    public static partial class InsqlMapperBuilderExtensions
    {
        public static InsqlMapperBuilder DisableXmlMapScan(this InsqlMapperBuilder builder)
        {
            builder.Services.Configure<InsqlModelOptions>(options =>
            {
                options.XmlMapEnabled = false;
            });

            return builder;
        }
    }
}

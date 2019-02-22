using Insql.Mappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Insql
{
    public static partial class InsqlBuilderExtensions
    {
        public static IInsqlBuilder AddDefaultDescriptorMapper(this IInsqlBuilder builder)
        {
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IInsqlDescriptorMapper, DapperDescriptorMapper>());

            return builder;
        }
    }
}

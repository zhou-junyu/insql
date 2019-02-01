using Insql.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Insql
{
    public static partial class InsqlBuilderExtensions
    {
        public static IInsqlBuilder AddProvider(this IInsqlBuilder builder, IInsqlDescriptorProvider provider)
        {
            builder.Services.AddSingleton(provider);

            return builder;
        }

        public static IInsqlBuilder ClearProviders(this IInsqlBuilder builder)
        {
            builder.Services.RemoveAll<IInsqlDescriptorProvider>();

            return builder;
        }
    }
}

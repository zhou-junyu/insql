using Insql.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Insql
{
    public static partial class InsqlProviderBuilderExtensions
    {
        public static IInsqlProviderBuilder AddProvider(this IInsqlProviderBuilder builder, IInsqlDescriptorProvider provider)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IInsqlDescriptorProvider>(provider));

            return builder;
        }

        public static IInsqlProviderBuilder AddProvider<T>(this IInsqlProviderBuilder builder) where T : class, IInsqlDescriptorProvider
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IInsqlDescriptorProvider, T>());

            return builder;
        }

        public static IInsqlProviderBuilder ClearProviders(this IInsqlProviderBuilder builder)
        {
            builder.Services.RemoveAll<IInsqlDescriptorProvider>();

            return builder;
        }
    }
}

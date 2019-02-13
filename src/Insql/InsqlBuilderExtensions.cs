using Insql.Providers;
using Insql.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Insql
{
    public static partial class InsqlBuilderExtensions
    {
        public static IInsqlBuilder AddDescriptorProvider(this IInsqlBuilder builder, IInsqlDescriptorProvider provider)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton(provider));

            return builder;
        }

        public static IInsqlBuilder AddDescriptorProvider<T>(this IInsqlBuilder builder) where T : class, IInsqlDescriptorProvider
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IInsqlDescriptorProvider, T>());

            return builder;
        }

        public static IInsqlBuilder ClearDescriptorProviders(this IInsqlBuilder builder)
        {
            builder.Services.RemoveAll<IInsqlDescriptorProvider>();

            return builder;
        }

        public static IInsqlBuilder AddResolveFilter(this IInsqlBuilder builder, ISqlResolveFilter resolveFilter)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton(resolveFilter));

            return builder;
        }

        public static IInsqlBuilder AddResolveFilter<T>(this IInsqlBuilder builder) where T : class, ISqlResolveFilter
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ISqlResolveFilter, T>());

            return builder;
        }

        public static IInsqlBuilder ClearResolveFilters(this IInsqlBuilder builder)
        {
            builder.Services.RemoveAll<ISqlResolveFilter>();

            return builder;
        }
    }
}

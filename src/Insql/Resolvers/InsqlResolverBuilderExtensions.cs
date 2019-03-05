using Insql.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Insql
{
    public static partial class InsqlResolverBuilderExtensions
    {
        public static IInsqlResolverBuilder AddFilter(this IInsqlResolverBuilder builder, IInsqlResolveFilter filter)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton(filter));

            return builder;
        }

        public static IInsqlResolverBuilder AddFilter<T>(this IInsqlResolverBuilder builder) where T : class, IInsqlResolveFilter
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IInsqlResolveFilter, T>());

            return builder;
        }

        public static IInsqlResolverBuilder ClearFilters(this IInsqlResolverBuilder builder)
        {
            builder.Services.RemoveAll<IInsqlResolveFilter>();

            return builder;
        }
    }
}

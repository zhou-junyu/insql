using Insql.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Insql
{
    public static partial class InsqlResolverBuilderExtensions
    {
        public static InsqlResolverBuilder AddFilter(this InsqlResolverBuilder builder, IInsqlResolveFilter filter)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton(filter));

            return builder;
        }

        public static InsqlResolverBuilder AddFilter<T>(this InsqlResolverBuilder builder) where T : class, IInsqlResolveFilter
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IInsqlResolveFilter, T>());

            return builder;
        }

        public static InsqlResolverBuilder ClearFilters(this InsqlResolverBuilder builder)
        {
            builder.Services.RemoveAll<IInsqlResolveFilter>();

            return builder;
        }
    }
}

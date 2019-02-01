using Insql;
using Insql.Resolvers;
using Insql.Resolvers.Codes;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InsqlServiceCollectionExtensions
    {
        public static IServiceCollection AddInsql(this IServiceCollection services, Action<IInsqlBuilder> configure)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddOptions();

            services.TryAdd(ServiceDescriptor.Singleton<ISqlResolverFactory, SqlResolverFactory>());
            services.TryAdd(ServiceDescriptor.Singleton(typeof(ISqlResolver<>), typeof(SqlResolver<>)));
            services.TryAdd(ServiceDescriptor.Singleton<IInsqlCodeResolver, JavaScriptCodeResolver>());

            configure(new InsqlBuilder(services));

            return services;
        }
    }
}

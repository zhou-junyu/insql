using Insql;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InsqlServiceCollectionExtensions
    {
        public static IServiceCollection AddInsql(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return services.AddInsql(builder => { });
        }

        public static IServiceCollection AddInsql(this IServiceCollection services, Action<IInsqlBuilder> configure, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            services.AddOptions();

            services.TryAdd(ServiceDescriptor.Singleton<IDbContextFactory, DbContextFactory>());
            services.TryAdd(ServiceDescriptor.Describe(typeof(IDbContext<>), typeof(DbContext<>), lifetime));

            configure(new InsqlBuilder(services).AddProvider().AddResolver().AddMapper());

            return services;
        }
    }
}

using Insql.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Insql
{
    public static class DbContextServiceCollectionExtensions
    {
        public static IServiceCollection AddInsqlDbContext<TContext>(this IServiceCollection services, Action<DbContextOptions<TContext>> options = null, ServiceLifetime lifetime = ServiceLifetime.Scoped)
          where TContext : DbContext
        {
            var contextType = typeof(TContext);

            services.Add(new ServiceDescriptor(typeof(DbContextOptions<TContext>), (serviceProvider) =>
            {
                var contextOptions = ActivatorUtilities.CreateInstance<DbContextOptions<TContext>>(serviceProvider);

                options?.Invoke(contextOptions);

                if (contextOptions.SqlResolver == null)
                {
                    contextOptions.SqlResolver = serviceProvider.GetService<ISqlResolver<TContext>>();
                }

                return contextOptions;
            }, lifetime));

            services.Add(new ServiceDescriptor(contextType, contextType, lifetime));

            return services;
        }
    }
}

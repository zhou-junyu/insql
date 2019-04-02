using Insql.Mappers;
using Insql.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Insql
{
    public static class DbContextServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContext<TContext>(this IServiceCollection services,
            Action<DbContextOptions<TContext>> options = null,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime optionsLifetime = ServiceLifetime.Singleton)
          where TContext : DbContext
        {
            var contextType = typeof(TContext);

            services.Add(new ServiceDescriptor(typeof(DbContextOptions<TContext>), (serviceProvider) =>
            {
                var contextOptions = new DbContextOptions<TContext>
                {
                    ServiceProvider = serviceProvider
                };

                options?.Invoke(contextOptions);

                if (contextOptions.Model == null)
                {
                    contextOptions.Model = serviceProvider.GetRequiredService<IInsqlModel>();
                }
                if (contextOptions.Resolver == null)
                {
                    contextOptions.Resolver = (IInsqlResolver)serviceProvider.GetRequiredService(typeof(IInsqlResolver<TContext>));
                }

                return contextOptions;
            }, optionsLifetime));

            services.Add(new ServiceDescriptor(contextType, contextType, contextLifetime));

            return services;
        }
    }
}

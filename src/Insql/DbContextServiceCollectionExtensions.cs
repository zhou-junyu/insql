using Insql.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Insql
{
    public static class DbContextServiceCollectionExtensions
    {
        public static IServiceCollection AddInsqlDbContext<TContext>(this IServiceCollection services,
            Action<DbContextOptions<TContext>> options = null,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime optionsLifetime = ServiceLifetime.Singleton)
          where TContext : DbContext
        {
            var contextType = typeof(TContext);

            services.Add(new ServiceDescriptor(typeof(DbContextOptions<TContext>), (serviceProvider) =>
            {
                var contextOptions = ActivatorUtilities.CreateInstance<DbContextOptions<TContext>>(serviceProvider);

                options?.Invoke(contextOptions);

                //todo : remove to DbContext OnConfiging After
                if (contextOptions.SqlResolver == null)
                {
                    contextOptions.SqlResolver = serviceProvider.TryGetService<ISqlResolver<TContext>>();
                }

                return contextOptions;
            }, optionsLifetime));

            services.Add(new ServiceDescriptor(contextType, contextType, contextLifetime));

            return services;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;

namespace Insql
{
    public static class DbContextServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContext<TContext>(this IServiceCollection services,
            Action<DbContextOptionsBuilder<TContext>> optionsConfigure = null,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime optionsLifetime = ServiceLifetime.Singleton)
          where TContext : DbContext
        {
            var contextType = typeof(TContext);

            services.Add(new ServiceDescriptor(typeof(DbContextOptions<TContext>), (serviceProvider) =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<TContext>(new DbContextOptions<TContext>
                {
                    ServiceProvider = serviceProvider
                });

                optionsConfigure?.Invoke(optionsBuilder);

                if (optionsBuilder.Options.Resolver == null)
                {
                    optionsBuilder.UseResolver<TContext>();
                }

                return optionsBuilder.Options;
            }, optionsLifetime));

            services.Add(new ServiceDescriptor(contextType, contextType, contextLifetime));

            return services;
        }

        [Obsolete]
        public static IServiceCollection AddInsqlDbContext<TContext>(this IServiceCollection services,
            Action<DbContextOptionsBuilder<TContext>> optionsConfigure = null,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime optionsLifetime = ServiceLifetime.Singleton)
          where TContext : DbContext
        {
            return services.AddDbContext<TContext>(optionsConfigure, contextLifetime, optionsLifetime);
        }
    }
}

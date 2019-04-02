using Insql;
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

        public static IServiceCollection AddInsql(this IServiceCollection services, Action<IInsqlBuilder> configure)
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
            
            configure(new InsqlBuilder(services).AddProvider().AddResolver().AddMapper());

            return services;
        }
    }
}

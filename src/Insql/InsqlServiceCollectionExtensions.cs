using Insql;
using Insql.Providers;
using Insql.Providers.EmbeddedXml;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
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

            services.TryAdd(ServiceDescriptor.Singleton<IInsqlFactory, InsqlFactory>());
            services.TryAdd(ServiceDescriptor.Describe(typeof(IInsql<>), typeof(InsqlImpl<>), lifetime));
            
            configure(new InsqlBuilder(services).AddEmbeddedXml().AddResolver().AddMapper());

            return services;
        }
    }
}

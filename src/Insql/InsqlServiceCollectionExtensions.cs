using Insql;
using Insql.Providers;
using Insql.Providers.EmbeddedXml;
using Insql.Resolvers;
using Insql.Resolvers.Matchers;
using Insql.Resolvers.Scripts;
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

            services.AddOptions();

            //sql resolver
            services.TryAdd(ServiceDescriptor.Singleton<ISqlResolveMatcher, DefaultResolveMatcher>());
            services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<DefaultResolveMatcherOptions>, DefaultResolveMatcherOptionsSetup>());

            services.TryAdd(ServiceDescriptor.Singleton<ISqlResolverFactory, SqlResolverFactory>());
            services.TryAdd(ServiceDescriptor.Singleton(typeof(ISqlResolver<>), typeof(SqlResolver<>)));

            //script resolver
            services.TryAdd(ServiceDescriptor.Singleton<IInsqlScriptResolver, DefaultScriptResolver>());
            services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<DefaultScriptResolverOptions>, DefaultScriptResolverOptionsSetup>());

            //descriptor provider
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IInsqlDescriptorProvider, EmbeddedDescriptorProvider>());
            services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<EmbeddedDescriptorOptions>, EmbeddedDescriptorOptionsSetup>());

            return services;
        }

        public static IServiceCollection AddInsql(this IServiceCollection services, Action<IInsqlBuilder> configure)
        {
            services.AddInsql();

            configure(new InsqlBuilder(services));

            return services;
        }
    }
}

using Insql;
using Insql.Providers;
using Insql.Providers.Embedded;
using Insql.Resolvers;
using Insql.Resolvers.Codes;
using Insql.Resolvers.Matchers;
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
            services.TryAdd(ServiceDescriptor.Singleton<IInsqlSectionMatcher, DefaultSectionMatcher>());
            services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<DefaultSectionMatcherOptions>, DefaultSectionMatcherOptionsSetup>());

            services.TryAdd(ServiceDescriptor.Singleton<ISqlResolverFactory, SqlResolverFactory>());
            services.TryAdd(ServiceDescriptor.Singleton(typeof(ISqlResolver<>), typeof(SqlResolver<>)));

            //code resolver
            services.TryAdd(ServiceDescriptor.Singleton<IInsqlCodeResolver, ScriptCodeResolver>());
            services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<ScriptCodeResolverOptions>, ScriptCodeResolverOptionsSetup>());

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

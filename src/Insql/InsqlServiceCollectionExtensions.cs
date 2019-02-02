using Insql;
using Insql.Providers;
using Insql.Providers.Embedded;
using Insql.Resolvers;
using Insql.Resolvers.Codes;
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
            services.TryAdd(ServiceDescriptor.Singleton<ISqlResolverFactory, SqlResolverFactory>());
            services.TryAdd(ServiceDescriptor.Singleton(typeof(ISqlResolver<>), typeof(SqlResolver<>)));

            //code resolver
            services.TryAdd(ServiceDescriptor.Singleton<IInsqlCodeResolver, JavaScriptCodeResolver>());
            services.TryAdd(ServiceDescriptor.Singleton<IOptions<JavascriptCodeResolverOptions>>((sp) => Options.Options.Create(new JavascriptCodeResolverOptions())));

            //descriptor provider
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IInsqlDescriptorProvider, EmbeddedDescriptorProvider>());
            services.TryAdd(ServiceDescriptor.Singleton<IOptions<EmbeddedDescriptorOptions>>((sp) => Options.Options.Create(new EmbeddedDescriptorOptions())));

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

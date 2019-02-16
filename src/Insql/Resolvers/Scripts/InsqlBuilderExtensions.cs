using Insql.Resolvers.Scripts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Insql
{
    public static partial class InsqlBuilderExtensions
    {
        public static IInsqlBuilder AddScriptResolver(this IInsqlBuilder builder, IInsqlScriptResolver codeResolver)
        {
            builder.Services.RemoveAll<IInsqlScriptResolver>();

            builder.Services.AddSingleton<IInsqlScriptResolver>(codeResolver);

            return builder;
        }

        public static IInsqlBuilder AddScriptResolver<T>(this IInsqlBuilder builder) where T : class, IInsqlScriptResolver
        {
            builder.Services.RemoveAll<IInsqlScriptResolver>();

            builder.Services.AddSingleton<IInsqlScriptResolver, T>();

            return builder;
        }

        public static IInsqlBuilder AddDefaultScriptResolver(this IInsqlBuilder builder, Action<DefaultScriptResolverOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.Services.TryAdd(ServiceDescriptor.Singleton<IInsqlScriptResolver, DefaultScriptResolver>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<DefaultScriptResolverOptions>, DefaultScriptResolverOptionsSetup>());

            builder.Services.Configure(configure);

            return builder;
        }
    }
}

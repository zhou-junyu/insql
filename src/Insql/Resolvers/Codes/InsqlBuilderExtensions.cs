using Insql.Resolvers.Codes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Insql
{
    public static partial class InsqlBuilderExtensions
    {
        public static IInsqlBuilder AddCodeResolver(this IInsqlBuilder builder, IInsqlCodeResolver codeResolver)
        {
            builder.Services.RemoveAll<IInsqlCodeResolver>();

            builder.Services.AddSingleton<IInsqlCodeResolver>(codeResolver);

            return builder;
        }

        public static IInsqlBuilder AddCodeResolver<T>(this IInsqlBuilder builder) where T : class, IInsqlCodeResolver
        {
            builder.Services.RemoveAll<IInsqlCodeResolver>();

            builder.Services.AddSingleton<IInsqlCodeResolver, T>();

            return builder;
        }

        public static IInsqlBuilder AddScriptCodeResolver(this IInsqlBuilder builder, Action<ScriptCodeResolverOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.Services.TryAdd(ServiceDescriptor.Singleton<IInsqlCodeResolver, ScriptCodeResolver>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<ScriptCodeResolverOptions>, ScriptCodeResolverOptionsSetup>());

            builder.Services.Configure(configure);

            return builder;
        }
    }
}

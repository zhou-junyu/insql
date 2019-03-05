using Insql.Resolvers;
using Insql.Resolvers.Scripts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Insql
{
    public static partial class InsqlResolverBuilderExtensions
    {
        public static IInsqlResolverBuilder AddDefaultScripter(this IInsqlResolverBuilder builder, Action<DefaultResolveScripterOptions> configure)
        {
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IInsqlResolveScripter, DefaultResolveScripter>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<DefaultResolveScripterOptions>, DefaultResolveScripterOptionsSetup>());

            if (configure != null)
            {
                builder.Services.Configure(configure);
            }

            return builder;
        }
    }
}

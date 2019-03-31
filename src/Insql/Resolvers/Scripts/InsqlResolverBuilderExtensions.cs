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
        public static IInsqlResolverBuilder AddScripter(this IInsqlResolverBuilder builder, Action<ResolveScripterOptions> configure)
        {
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IInsqlResolveScripter, ResolveScripter>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<ResolveScripterOptions>, ResolveScripterOptionsSetup>());

            if (configure != null)
            {
                builder.Services.Configure(configure);
            }
            
            return builder;
        }
    }
}

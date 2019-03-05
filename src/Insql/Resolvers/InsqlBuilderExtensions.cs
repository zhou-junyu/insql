using Insql.Resolvers;
using Insql.Resolvers.Matchers;
using Insql.Resolvers.Scripts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Insql
{
    public static partial class InsqlBuilderExtensions
    {
        public static IInsqlBuilder AddResolver(this IInsqlBuilder builder)
        {
            builder.Services.TryAdd(ServiceDescriptor.Singleton(typeof(IInsqlResolver<>), typeof(InsqlResolver<>)));
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IInsqlResolverFactory, InsqlResolverFactory>());

            builder.Services.TryAdd(ServiceDescriptor.Singleton<IInsqlResolveMatcher, DefaultResolveMatcher>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<DefaultResolveMatcherOptions>, DefaultResolveMatcherOptionsSetup>());

            builder.Services.TryAdd(ServiceDescriptor.Singleton<IInsqlResolveScripter, DefaultResolveScripter>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<DefaultResolveScripterOptions>, DefaultResolveScripterOptionsSetup>());
            
            return builder;
        }

        public static IInsqlBuilder AddResolver(this IInsqlBuilder builder, Action<IInsqlResolverBuilder> configure)
        {
            builder.AddResolver();

            configure(new InsqlResolverBuilder(builder.Services));

            return builder;
        }
    }
}

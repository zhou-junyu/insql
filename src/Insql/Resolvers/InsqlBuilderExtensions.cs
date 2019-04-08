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

            builder.Services.TryAdd(ServiceDescriptor.Singleton<IInsqlResolveMatcher, ResolveMatcher>());

            builder.Services.TryAdd(ServiceDescriptor.Singleton<IInsqlResolveScripter, ResolveScripter>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<ResolveScripterOptions>, ResolveScripterOptionsSetup>());
            
            return builder;
        }

        public static IInsqlBuilder AddResolver(this IInsqlBuilder builder, Action<InsqlResolverBuilder> configure)
        {
            builder.AddResolver();

            configure(new InsqlResolverBuilder(builder.Services));

            return builder;
        }
    }
}

using Insql.Resolvers;
using Insql.Resolvers.Matchers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Insql
{
    public static partial class InsqlResolverBuilderExtensions
    {
        public static IInsqlResolverBuilder AddDefaultMatcher(this IInsqlResolverBuilder builder, Action<DefaultResolveMatcherOptions> configure)
        {
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IInsqlResolveMatcher, DefaultResolveMatcher>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<DefaultResolveMatcherOptions>, DefaultResolveMatcherOptionsSetup>());

            if (configure != null)
            {
                builder.Services.Configure(configure);
            }

            return builder;
        }
    }
}

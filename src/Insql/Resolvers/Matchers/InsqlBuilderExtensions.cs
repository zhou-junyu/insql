using Insql.Resolvers;
using Insql.Resolvers.Matchers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Insql
{
    public static partial class InsqlBuilderExtensions
    {
        public static IInsqlBuilder AddResolveMatcher(this IInsqlBuilder builder, ISqlResolveMatcher sectionMatcher)
        {
            builder.Services.RemoveAll<ISqlResolveMatcher>();

            builder.Services.AddSingleton<ISqlResolveMatcher>(sectionMatcher);

            return builder;
        }

        public static IInsqlBuilder AddResolveMatcher<T>(this IInsqlBuilder builder) where T : class, ISqlResolveMatcher
        {
            builder.Services.RemoveAll<ISqlResolveMatcher>();

            builder.Services.AddSingleton<ISqlResolveMatcher, T>();

            return builder;
        }

        public static IInsqlBuilder AddDefaultResolveMatcher(this IInsqlBuilder builder, Action<DefaultResolveMatcherOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.Services.TryAdd(ServiceDescriptor.Singleton<ISqlResolveMatcher, DefaultResolveMatcher>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<DefaultResolveMatcherOptions>, DefaultResolveMatcherOptionsSetup>());

            builder.Services.Configure(configure);

            return builder;
        }
    }
}

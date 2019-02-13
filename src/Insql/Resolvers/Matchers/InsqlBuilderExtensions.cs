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
        public static IInsqlBuilder AddResolveMatcher(this IInsqlBuilder builder, IInsqlSectionMatcher sectionMatcher)
        {
            builder.Services.RemoveAll<IInsqlSectionMatcher>();

            builder.Services.AddSingleton<IInsqlSectionMatcher>(sectionMatcher);

            return builder;
        }

        public static IInsqlBuilder AddResolveMatcher<T>(this IInsqlBuilder builder) where T : class, IInsqlSectionMatcher
        {
            builder.Services.RemoveAll<IInsqlSectionMatcher>();

            builder.Services.AddSingleton<IInsqlSectionMatcher, T>();

            return builder;
        }

        public static IInsqlBuilder AddDefaultResolveMatcher(this IInsqlBuilder builder, Action<DefaultSectionMatcherOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.Services.TryAdd(ServiceDescriptor.Singleton<IInsqlSectionMatcher, DefaultSectionMatcher>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<DefaultSectionMatcherOptions>, DefaultSectionMatcherOptionsSetup>());

            builder.Services.Configure(configure);

            return builder;
        }
    }
}

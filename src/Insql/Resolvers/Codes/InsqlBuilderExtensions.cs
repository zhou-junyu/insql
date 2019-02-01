using Insql.Resolvers.Codes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Insql
{
    public static partial class InsqlBuilderExtensions
    {
        public static IInsqlBuilder AddJavaScriptCodeResolver(this IInsqlBuilder builder, Action<JavascriptCodeResolverOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IInsqlCodeResolver, JavaScriptCodeResolver>());

            builder.Services.Configure(configure);

            return builder;
        }
    }
}

using Insql.Resolvers.Codes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
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

            builder.Services.TryAdd(ServiceDescriptor.Singleton<IInsqlCodeResolver, JavaScriptCodeResolver>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IOptions<JavascriptCodeResolverOptions>>((sp) => Options.Create(new JavascriptCodeResolverOptions())));

            builder.Services.Configure(configure);

            return builder;
        }
    }
}

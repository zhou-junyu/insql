using Insql.Providers;
using Insql.Providers.EmbeddedXml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Insql
{
    public static partial class InsqlProviderBuilderExtensions
    {
        public static IInsqlProviderBuilder AddEmbeddedXml(this IInsqlProviderBuilder builder, Action<EmbeddedDescriptorOptions> configure)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IInsqlDescriptorProvider, EmbeddedDescriptorProvider>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<EmbeddedDescriptorOptions>, EmbeddedDescriptorOptionsSetup>());

            builder.Services.Configure(configure);

            return builder;
        }
    }
}

using Insql.Providers;
using Insql.Providers.ExternalXml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Insql
{
    public static partial class InsqlProviderBuilderExtensions
    {
        public static IInsqlProviderBuilder AddExternalXml(this IInsqlProviderBuilder builder, Action<ExternalDescriptorOptions> configure)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IInsqlDescriptorProvider, ExternalDescriptorProvider>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<ExternalDescriptorOptions>, ExternalDescriptorOptionsSetup>());

            builder.Services.Configure(configure);

            return builder;
        }
    }
}

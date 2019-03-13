using Insql.Providers;
using Insql.Providers.ExternalXml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Insql
{
    public static partial class InsqlBuilderExtensions
    {
        public static IInsqlBuilder AddExternalXml(this IInsqlBuilder builder)
        {
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IInsqlDescriptorLoader, InsqlDescriptorLoader>());

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IInsqlDescriptorProvider, ExternalDescriptorProvider>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<ExternalDescriptorOptions>, ExternalDescriptorOptionsSetup>());

            return builder;
        }

        public static IInsqlBuilder AddExternalXml(this IInsqlBuilder builder, Action<ExternalDescriptorOptions> configure)
        {
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IInsqlDescriptorLoader, InsqlDescriptorLoader>());

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IInsqlDescriptorProvider, ExternalDescriptorProvider>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<ExternalDescriptorOptions>, ExternalDescriptorOptionsSetup>());

            builder.Services.Configure(configure);

            return builder;
        }
    }
}

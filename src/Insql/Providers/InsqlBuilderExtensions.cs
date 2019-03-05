using Insql.Providers;
using Insql.Providers.EmbeddedXml;
using Insql.Providers.ExternalXml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Insql
{
    public static partial class InsqlBuilderExtensions
    {
        public static IInsqlBuilder AddProvider(this IInsqlBuilder builder)
        {
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IInsqlDescriptorLoader, InsqlDescriptorLoader>());

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IInsqlDescriptorProvider, EmbeddedDescriptorProvider>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<EmbeddedDescriptorOptions>, EmbeddedDescriptorOptionsSetup>());

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IInsqlDescriptorProvider, ExternalDescriptorProvider>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<ExternalDescriptorOptions>, ExternalDescriptorOptionsSetup>());

            return builder;
        }

        public static IInsqlBuilder AddProvider(this IInsqlBuilder builder, Action<IInsqlProviderBuilder> configre)
        {
            builder.AddProvider();

            configre(new InsqlProviderBuilder(builder.Services));

            return builder;
        }
    }
}

using Insql.Providers;
using Insql.Providers.DirectoryXml;
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
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IInsqlDescriptorProvider, ExternalDescriptorProvider>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<ExternalDescriptorOptions>, ExternalDescriptorOptionsSetup>());

            return builder;
        }

        public static IInsqlBuilder AddExternalXml(this IInsqlBuilder builder, Action<ExternalDescriptorOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddExternalXml();

            builder.Services.Configure(configure);

            return builder;
        }
    }
}

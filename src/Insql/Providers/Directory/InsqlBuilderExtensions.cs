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
        public static IInsqlBuilder AddDirectoryXml(this IInsqlBuilder builder)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IInsqlDescriptorProvider, DirectoryDescriptorProvider>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<DirectoryDescriptorOptions>, DirectoryDescriptorOptionsSetup>());

            return builder;
        }

        public static IInsqlBuilder AddDirectoryXml(this IInsqlBuilder builder, Action<DirectoryDescriptorOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddDirectoryXml();

            builder.Services.Configure(configure);

            return builder;
        }
    }
}

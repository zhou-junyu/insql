using Microsoft.Extensions.Options;

namespace Insql.Providers.DirectoryXml
{
    internal class ExternalDescriptorOptionsSetup : IConfigureOptions<ExternalDescriptorOptions>
    {
        public void Configure(ExternalDescriptorOptions options)
        {
            options.Enabled = true;
            options.Matches = "**/*.insql.xml";
            options.Namespace = "";
        }
    }
}

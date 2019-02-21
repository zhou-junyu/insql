using Microsoft.Extensions.Options;

namespace Insql.Providers.DirectoryXml
{
    public class DirectoryDescriptorOptionsSetup : IConfigureOptions<DirectoryDescriptorOptions>
    {
        public void Configure(DirectoryDescriptorOptions options)
        {
            options.Enabled = true;
            options.Matches = "**/*.insql.xml";
            options.Namespace = "";
        }
    }
}

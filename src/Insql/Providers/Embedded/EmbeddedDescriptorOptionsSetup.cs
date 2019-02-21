using Microsoft.Extensions.Options;

namespace Insql.Providers.EmbeddedXml
{
    public class EmbeddedDescriptorOptionsSetup : IConfigureOptions<EmbeddedDescriptorOptions>
    {
        public void Configure(EmbeddedDescriptorOptions options)
        {
            options.Enabled = true;
            options.Matches = "**/*.insql.xml";
            options.Namespace = "";
        }
    }
}

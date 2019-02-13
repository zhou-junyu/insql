using Microsoft.Extensions.Options;

namespace Insql.Providers.Embedded
{
    public class EmbeddedDescriptorOptionsSetup : IConfigureOptions<EmbeddedDescriptorOptions>
    {
        public void Configure(EmbeddedDescriptorOptions options)
        {
            options.Locations = "**/*.insql.xml";
            options.Namespace = "";
        }
    }
}

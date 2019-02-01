using Microsoft.Extensions.Options;

namespace Insql.Providers.Embedded
{
    public class EmbeddedDescriptorOptionsSetup : ConfigureOptions<EmbeddedDescriptorOptions>
    {
        public EmbeddedDescriptorOptionsSetup() : base(action =>
        {
            action.Locations = "**/*.insql.xml";
            action.Namespace = string.Empty;
        })
        {
        }
    }
}

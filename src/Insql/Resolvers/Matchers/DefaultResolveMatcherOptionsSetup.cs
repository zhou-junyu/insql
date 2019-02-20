using Microsoft.Extensions.Options;

namespace Insql.Resolvers.Matchers
{
    public class DefaultResolveMatcherOptionsSetup : IConfigureOptions<DefaultResolveMatcherOptions>
    {
        public void Configure(DefaultResolveMatcherOptions options)
        {
            options.CorssDbEnabled = true;
            options.CorssDbSeparator = ".";
        }
    }
}

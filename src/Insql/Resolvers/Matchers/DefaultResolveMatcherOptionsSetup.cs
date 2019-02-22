using Microsoft.Extensions.Options;

namespace Insql.Resolvers.Matchers
{
    internal class DefaultResolveMatcherOptionsSetup : IConfigureOptions<DefaultResolveMatcherOptions>
    {
        public void Configure(DefaultResolveMatcherOptions options)
        {
            options.CorssDbEnabled = true;
            options.CorssDbSeparator = ".";
        }
    }
}

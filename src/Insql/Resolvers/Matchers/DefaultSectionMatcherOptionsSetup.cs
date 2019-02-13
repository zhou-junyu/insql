using Microsoft.Extensions.Options;

namespace Insql.Resolvers.Matchers
{
    public class DefaultSectionMatcherOptionsSetup : IConfigureOptions<DefaultSectionMatcherOptions>
    {
        public void Configure(DefaultSectionMatcherOptions options)
        {
            options.CorssDbEnabled = true;
            options.CorssDbSeparator = ".";
        }
    }
}

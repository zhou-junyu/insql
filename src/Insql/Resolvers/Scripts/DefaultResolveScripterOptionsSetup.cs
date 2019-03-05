using Microsoft.Extensions.Options;

namespace Insql.Resolvers.Scripts
{
    internal class DefaultResolveScripterOptionsSetup : IConfigureOptions<DefaultResolveScripterOptions>
    {
        public void Configure(DefaultResolveScripterOptions options)
        {
            options.IsConvertEnum = true;
            options.IsConvertOperator = true;
            options.IsConvertDateTimeMin = true;
        }
    }
}

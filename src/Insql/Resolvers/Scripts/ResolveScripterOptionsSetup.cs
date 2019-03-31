using Microsoft.Extensions.Options;

namespace Insql.Resolvers.Scripts
{
    internal class ResolveScripterOptionsSetup : IConfigureOptions<ResolveScripterOptions>
    {
        public void Configure(ResolveScripterOptions options)
        {
            options.IsConvertEnum = true;
            options.IsConvertOperator = true;
            options.IsConvertDateTimeMin = true;
        }
    }
}

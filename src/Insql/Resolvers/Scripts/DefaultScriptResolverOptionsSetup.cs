using Microsoft.Extensions.Options;

namespace Insql.Resolvers.Scripts
{
    public class DefaultScriptResolverOptionsSetup : IConfigureOptions<DefaultScriptResolverOptions>
    {
        public void Configure(DefaultScriptResolverOptions options)
        {
            options.IsConvertEnum = true;
            options.IsConvertOperator = true;
            options.IsConvertDateTimeMin = true;
        }
    }
}

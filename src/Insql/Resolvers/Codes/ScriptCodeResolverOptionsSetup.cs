using Microsoft.Extensions.Options;

namespace Insql.Resolvers.Codes
{
    public class ScriptCodeResolverOptionsSetup : IConfigureOptions<ScriptCodeResolverOptions>
    {
        public void Configure(ScriptCodeResolverOptions options)
        {
            options.IsConvertEnum = true;
            options.IsReplaceOperator = true;
        }
    }
}

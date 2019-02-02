using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Insql.Resolvers.Codes
{
    public class JavascriptCodeResolverOptionsSetup : ConfigureOptions<JavascriptCodeResolverOptions>
    {
        public JavascriptCodeResolverOptionsSetup() : base(action =>
        {
            action.IsConvertEnum = true;

            action.IsReplaceOperator = true;

            action.OperatorMappings = new Dictionary<string, string>
            {
                { "and","&&" },
                { "or","||" },
                { "gt",">" },
                { "gte",">=" },
                { "lt","<" },
                { "lte","<=" },
                //{ "eq","==" },
                //{ "neq","!=" },
            };
        })
        {
        }
    }
}

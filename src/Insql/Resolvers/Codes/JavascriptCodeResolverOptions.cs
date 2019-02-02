using System.Collections.Generic;

namespace Insql.Resolvers.Codes
{
    public class JavascriptCodeResolverOptions
    {
        public bool IsConvertEnum { get; set; } = true;

        public bool IsReplaceOperator { get; set; } = true;

        public Dictionary<string, string> OperatorMappings { get; } = new Dictionary<string, string>
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
    }
}

using System.Collections.Generic;

namespace Insql.Resolvers.Codes
{
    public class JavascriptCodeResolverOptions
    {
        public bool IsConvertEnum { get; set; }

        public bool IsReplaceOperator { get; set; }

        public Dictionary<string, string> OperatorMappings { get; set; }
    }
}

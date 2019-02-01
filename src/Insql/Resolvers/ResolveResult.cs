using System.Collections.Generic;

namespace Insql.Resolvers
{
    public class ResolveResult
    {
        public string Sql { get; set; }

        public IDictionary<string, object> Param { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Insql.Resolvers
{
    public class ResolveContext
    {
        public IServiceProvider ServiceProvider { get; set; }

        public InsqlDescriptor InsqlDescriptor { get; set; }

        public InsqlSection InsqlSection { get; set; }

        public IDictionary<string, object> Param { get; set; }
    }
}

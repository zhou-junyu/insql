using System;
using System.Collections.Generic;

namespace Insql.Internal
{
    internal class InsqlOptions<TContext> : InsqlOptions where TContext : IInsql
    {
        public InsqlOptions() : base(new Dictionary<Type, IInsqlOptionsExtension>())
        {
        }

        public override Type Type => typeof(TContext);
    }
}

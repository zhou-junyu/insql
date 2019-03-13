using System;
using System.Collections.Generic;

namespace Insql.Internal
{
    internal abstract class InsqlOptions : IInsqlOptions
    {
        public abstract Type Type { get; }
    }
}

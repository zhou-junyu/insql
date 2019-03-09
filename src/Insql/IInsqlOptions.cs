using System;
using System.Collections.Generic;

namespace Insql
{
    public interface IInsqlOptions
    {
        Type Type { get; }

        IEnumerable<IInsqlOptionsExtension> Extensions { get; }

        TExtension FindExtension<TExtension>() where TExtension : class, IInsqlOptionsExtension;
    }
}

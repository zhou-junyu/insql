using System;

namespace Insql
{
    public interface IInsqlOptions
    {
        Type Type { get; }

        TExtension FindExtension<TExtension>() where TExtension : class;
    }
}

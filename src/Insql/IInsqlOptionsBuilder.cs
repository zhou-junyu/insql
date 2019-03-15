using System;

namespace Insql
{
    public interface IInsqlOptionsBuilder
    {
        Type Type { get; }

        IInsqlOptions Options { get; }

        IInsqlOptionsBuilder UseExtension<TExtension>(TExtension extension) where TExtension : class, IInsqlOptionsExtension;
    }
}

using System;

namespace Insql
{
    public interface IInsqlOptionsBuilder
    {
        Type ContextType { get; }

        IInsqlOptionsBuilder UseExtension<TExtension>(TExtension extension) where TExtension : class;
    }
}

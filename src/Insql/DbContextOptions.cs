using System;
using System.Collections.Generic;

namespace Insql
{
    public abstract class DbContextOptions : IInsqlOptions
    {
        public abstract Type Type { get; }

        public IEnumerable<IInsqlOptionsExtension> Extensions => throw new NotImplementedException();

        public TExtension FindExtension<TExtension>()
            where TExtension : class, IInsqlOptionsExtension
        {
            throw new NotImplementedException();
        }

        public abstract void WithExtension<TExtension>(TExtension extension)
            where TExtension : class, IInsqlOptionsExtension;
    }
}

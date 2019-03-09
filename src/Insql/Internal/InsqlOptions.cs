using System;
using System.Collections.Generic;

namespace Insql.Internal
{
    internal abstract class InsqlOptions : IInsqlOptions
    {
        private readonly IDictionary<Type, IInsqlOptionsExtension> extensions;

        public InsqlOptions(IDictionary<Type, IInsqlOptionsExtension> extensions)
        {
            this.extensions = extensions;
        }

        public abstract Type Type { get; }

        public IEnumerable<IInsqlOptionsExtension> Extensions => this.extensions.Values;

        TExtension IInsqlOptions.FindExtension<TExtension>()
        {
            if (this.extensions.TryGetValue(typeof(TExtension), out var extension))
            {
                return (TExtension)extension;
            }

            return null;
        }
    }
}

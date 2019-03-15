using System;

namespace Insql
{
    internal class InsqlOptions : IInsqlOptions
    {
        public InsqlOptions(Type type)
        {
            this.Type = type;
        }

        public Type Type { get; }

        public TExtension FindExtension<TExtension>() where TExtension : class
        {
            throw new NotImplementedException();
        }
    }
}

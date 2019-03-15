using System;
using System.Collections.Generic;
using System.Text;

namespace Insql
{
    internal class InsqlOptionsBuilder : IInsqlOptionsBuilder
    {
        public InsqlOptionsBuilder(Type type)
        {
            this.Options = new InsqlOptions()
        }

        public Type Type => throw new NotImplementedException();

        public IInsqlOptions Options { get; }

        public IInsqlOptionsBuilder UseExtension<TExtension>(TExtension extension) where TExtension : class
        {
            throw new NotImplementedException();
        }
    }
}

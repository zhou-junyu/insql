using Microsoft.Extensions.Options;
using System;

namespace Insql
{
    internal class InsqlOptions<TContext> : IInsqlOptions<TContext> where TContext : IInsql
    {
        public InsqlOptions(IOptions<InsqlOptionsBuilderConfigure> configureOptions)
        {

            //配置

            //new OptionsBuilder() configure 
        }

        public Type Type => throw new NotImplementedException();

        public TExtension FindExtension<TExtension>() where TExtension : class
        {
            throw new NotImplementedException();
        }
    }
}

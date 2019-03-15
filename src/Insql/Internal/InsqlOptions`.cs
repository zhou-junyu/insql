using Microsoft.Extensions.Options;
using System;

namespace Insql
{
    internal class InsqlOptions<T> : IInsqlOptions<T> where T : IInsql
    {
        private readonly IInsqlOptions options;

        public InsqlOptions(IOptions<InsqlOptionsBuilderConfigure> configureOptions)
        {
            var optionsBuilder = new InsqlOptionsBuilder(typeof(T));

            configureOptions.Value.Configure?.Invoke(optionsBuilder);

            this.options = optionsBuilder.Options;
        }

        public Type ContextType => this.options.Type;

        public TExtension FindExtension<TExtension>() where TExtension : class
        {
            return this.options.FindExtension<TExtension>();
        }
    }
}

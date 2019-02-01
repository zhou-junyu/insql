using Microsoft.Extensions.DependencyInjection;

namespace Insql
{
    internal class InsqlBuilder : IInsqlBuilder
    {
        public InsqlBuilder(IServiceCollection services)
        {
            this.Services = services;
        }

        public IServiceCollection Services { get; }
    }
}

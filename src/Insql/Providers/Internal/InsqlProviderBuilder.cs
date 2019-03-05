using Microsoft.Extensions.DependencyInjection;

namespace Insql.Providers
{
    internal class InsqlProviderBuilder : IInsqlProviderBuilder
    {
        public InsqlProviderBuilder(IServiceCollection services)
        {
            this.Services = services;
        }

        public IServiceCollection Services { get; }
    }
}

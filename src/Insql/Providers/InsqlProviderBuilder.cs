using Microsoft.Extensions.DependencyInjection;

namespace Insql.Providers
{
    public class InsqlProviderBuilder
    {
        public InsqlProviderBuilder(IServiceCollection services)
        {
            this.Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
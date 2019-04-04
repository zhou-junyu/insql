using Microsoft.Extensions.DependencyInjection;

namespace Insql.Resolvers
{
    public class InsqlResolverBuilder
    {
        public InsqlResolverBuilder(IServiceCollection services)
        {
            this.Services = services;
        }

        public IServiceCollection Services { get; }
    }
}

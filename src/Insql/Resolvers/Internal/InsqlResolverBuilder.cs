using Microsoft.Extensions.DependencyInjection;

namespace Insql.Resolvers
{
    internal class InsqlResolverBuilder : IInsqlResolverBuilder
    {
        public InsqlResolverBuilder(IServiceCollection services)
        {
            this.Services = services;
        }

        public IServiceCollection Services { get; }
    }
}

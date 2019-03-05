using Microsoft.Extensions.DependencyInjection;

namespace Insql.Mappers
{
    internal class InsqlMapperBuilder : IInsqlMapperBuilder
    {
        public InsqlMapperBuilder(IServiceCollection services)
        {
            this.Services = services;
        }

        public IServiceCollection Services { get; }
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace Insql.Mappers
{
    public class InsqlMapperBuilder
    {
        public InsqlMapperBuilder(IServiceCollection services)
        {
            this.Services = services;
        }

        public IServiceCollection Services { get; }
    }
}

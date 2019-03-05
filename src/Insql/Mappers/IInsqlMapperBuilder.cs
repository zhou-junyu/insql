using Microsoft.Extensions.DependencyInjection;

namespace Insql
{
    public interface IInsqlMapperBuilder
    {
        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> where Insql Providers services are configured.
        /// </summary>
        IServiceCollection Services { get; }
    }
}

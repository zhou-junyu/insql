using Microsoft.Extensions.DependencyInjection;

namespace Insql
{
    public interface IInsqlProviderBuilder
    {
        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> where Insql Providers services are configured.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
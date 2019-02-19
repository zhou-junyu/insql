using Microsoft.Extensions.DependencyInjection;
using System;

namespace Insql
{
    internal static class ServiceProviderServiceExtensions
    {
        public static T TryGetService<T>(this IServiceProvider provider) where T : class
        {
            try
            {
                return provider.GetService<T>();
            }
            catch
            {
                return null;
            }
        }
    }
}

using Insql.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace Insql
{
    public static partial class DbContextOptionsExtensions
    {
        public static DbContextOptions UseSqlResolver<T>(this DbContextOptions options) where T : class
        {
            options.SqlResolver = options.ServiceProvider.GetRequiredService<ISqlResolver<T>>();

            return options;
        }
    }
}

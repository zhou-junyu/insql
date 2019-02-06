using Insql.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace Insql
{
    public static partial class DbContextOptionsExtensions
    {
        public static DbContextOptions UseSqlResolver<TInsql>(this DbContextOptions options) where TInsql : class
        {
            options.SqlResolver = options.ServiceProvider.GetRequiredService<ISqlResolver<TInsql>>();

            return options;
        }
    }
}

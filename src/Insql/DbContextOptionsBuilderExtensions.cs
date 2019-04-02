using Insql.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace Insql
{
    public static partial class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseResolver<TContext>(this DbContextOptionsBuilder builder) where TContext : class
        {
            builder.Options.Resolver = builder.Options.ServiceProvider.GetRequiredService<IInsqlResolver<TContext>>();

            return builder;
        }
    }
}

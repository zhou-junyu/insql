using Insql.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Insql
{
    public static partial class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseServiceProvider(this DbContextOptionsBuilder builder, IServiceProvider serviceProvider)
        {
            builder.Options.ServiceProvider = serviceProvider;

            return builder;
        }

        public static DbContextOptionsBuilder UseResolver<TContext>(this DbContextOptionsBuilder builder) where TContext : class
        {
            builder.Options.Resolver = builder.Options.ServiceProvider.GetRequiredService<IInsqlResolver<TContext>>();

            return builder;
        }
    }
}

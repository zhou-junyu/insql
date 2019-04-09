using Insql.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Insql
{
    public static partial class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder CommandTimeout(this DbContextOptionsBuilder builder, int? commandTimeout)
        {
            builder.Options.CommandTimeout = commandTimeout;

            return builder;
        }

        public static DbContextOptionsBuilder UseResolver<TContext>(this DbContextOptionsBuilder builder) where TContext : class
        {
            builder.Options.Resolver = builder.Options.ServiceProvider.GetRequiredService<IInsqlResolver<TContext>>();

            return builder;
        }

        public static DbContextOptionsBuilder UseDialect(this DbContextOptionsBuilder builder, IDbDialect dialect)
        {
            if (dialect == null)
            {
                throw new ArgumentNullException(nameof(dialect));
            }

            builder.Options.Dialect = dialect;

            return builder;
        }
    }
}

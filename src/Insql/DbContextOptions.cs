using Insql.Resolvers;
using System;

namespace Insql
{
    public abstract class DbContextOptions
    {
        public abstract Type ContextType { get; }

        public IServiceProvider ServiceProvider { get; set; }

        public IInsqlResolver Resolver { get; set; }

        public IDbDialect Dialect { get; set; }

        public IDbSessionFactory SessionFactory { get; set; }

        public int? CommandTimeout { get; set; }

        public bool IsConfigured { get; set; }
    }
}

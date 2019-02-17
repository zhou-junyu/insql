using Insql.Resolvers;
using System;

namespace Insql
{
    public abstract class DbContextOptions
    {
        public abstract Type ContextType { get; }

        public IServiceProvider ServiceProvider { get; set; }

        public IDbSessionFactory SessionFactory { get; set; }

        public ISqlResolver SqlResolver { get; set; }

        public ResolveEnviron ResolveEnviron { get; }

        public int? CommandTimeout { get; set; }

        public DbContextOptions()
        {
            this.ResolveEnviron = new ResolveEnviron();
        }
    }
}

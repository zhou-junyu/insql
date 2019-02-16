using Insql.Resolvers;
using System;

namespace Insql
{
    public abstract class DbContextOptions
    {
        public abstract Type ContextType { get; }

        public IServiceProvider ServiceProvider { get; set; }

        public IDbSession DbSession { get; set; }

        public ISqlResolver SqlResolver { get; set; }

        public ResolveEnviron SqlResolveEnv { get; }

        public DbContextOptions()
        {
            this.SqlResolveEnv = new ResolveEnviron();
        }
    }
}

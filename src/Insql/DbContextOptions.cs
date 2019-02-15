using Insql.Resolvers;
using System;
using System.Collections.Generic;

namespace Insql
{
    public abstract class DbContextOptions
    {
        public abstract Type ContextType { get; }

        public virtual IServiceProvider ServiceProvider { get; }

        public IDbSessionFactory SessionFactory { get; set; }

        public ISqlResolver SqlResolver { get; set; }

        public IDictionary<string, string> SqlResolverEnvironment { get; }

        public int? CommandTimeout { get; set; }

        public DbContextOptions()
        {
            this.SqlResolverEnvironment = new Dictionary<string, string>();
        }
    }
}

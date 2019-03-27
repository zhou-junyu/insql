using Insql.Mappers;
using Insql.Resolvers;
using System;

namespace Insql
{
    public abstract class DbContextOptions
    {
        public abstract Type ContextType { get; }

        public IInsqlResolver Resolver { get; set; }

        public IInsqlModel Model { get; set; }

        public string DbType { get; set; }

        public IDbDialect DbDialect { get; set; }

        public IDbSessionFactory SessionFactory { get; set; }

        public IServiceProvider ServiceProvider { get; set; }
    }
}

using Insql.Mappers;
using Insql.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Insql
{
    internal class DbContextFactory : IDbContextFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IDbSessionFactory sessionFactory;
        private readonly IInsqlModel insqlModel;

        public DbContextFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            this.insqlModel = serviceProvider.GetRequiredService<IInsqlModel>();
            this.sessionFactory = serviceProvider.GetRequiredService<IDbSessionFactory>();
        }

        public IDbContext CreateContext(Type contextType)
        {
            var insqlResolver = (IInsqlResolver)this.serviceProvider.GetRequiredService(typeof(IInsqlResolver<>).MakeGenericType(contextType));

            return new DbContext(contextType, this.insqlModel, insqlResolver, this.sessionFactory);
        }
    }
}

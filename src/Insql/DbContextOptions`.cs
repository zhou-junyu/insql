using System;

namespace Insql
{
    public class DbContextOptions<TContext> : DbContextOptions
        where TContext : DbContext
    {
        private readonly IServiceProvider serviceProvider;

        public DbContextOptions(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public override Type ContextType => typeof(TContext);

        public override IServiceProvider ServiceProvider => this.serviceProvider;
    }
}

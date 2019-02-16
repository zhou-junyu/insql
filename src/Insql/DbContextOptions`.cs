using System;

namespace Insql
{
    public class DbContextOptions<TContext> : DbContextOptions
        where TContext : DbContext
    {
        public DbContextOptions(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public override Type ContextType => typeof(TContext);
    }
}

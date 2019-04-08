using System;

namespace Insql
{
    public class DbContextOptions<TContext> : DbContextOptions
        where TContext : DbContext
    {
        public override Type ContextType => typeof(TContext);
    }
}

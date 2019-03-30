namespace Insql
{
    public class DbContextOptionsBuilder<TContext> : DbContextOptionsBuilder
        where TContext : DbContext
    {
        public DbContextOptionsBuilder() : base(new DbContextOptions<TContext>())
        {
        }
    }
}

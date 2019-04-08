namespace Insql
{
    public class DbContextOptionsBuilder<TContext> : DbContextOptionsBuilder
        where TContext : DbContext
    {
        public DbContextOptionsBuilder() : base(new DbContextOptions<TContext>())
        {
        }

        public DbContextOptionsBuilder(DbContextOptions options) : base(options)
        {
        }
    }
}

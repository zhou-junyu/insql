namespace Insql.Tests.Contexts
{
    public class CommonDbContext<T> : DbContext where T : class
    {
        public CommonDbContext(DbContextOptions<DbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseResolver<T>();
        }
    }
}

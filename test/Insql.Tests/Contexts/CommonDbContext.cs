namespace Insql.Tests.Contexts
{
    public class CommonDbContext<T> : DbContext where T : class
    {
        public CommonDbContext(CommonDbContextOptions<T> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseResolver<T>();

            optionsBuilder.UseSqlite("Data Source= ./insql.tests.db");
        }
    }
}

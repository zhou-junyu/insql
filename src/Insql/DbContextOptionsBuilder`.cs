namespace Insql
{
    public class DbContextOptionsBuilder<T> : DbContextOptionsBuilder
        where T : DbContext
    {
        public DbContextOptionsBuilder() : base(typeof(T))
        {
        }
    }
}

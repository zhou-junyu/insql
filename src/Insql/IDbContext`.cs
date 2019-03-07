namespace Insql
{
    public interface IDbContext<TContext> : IDbContext
        where TContext : class
    {
    }
}

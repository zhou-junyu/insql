namespace Insql
{
    public interface IInsql<TContext> : IInsql
        where TContext : class
    {
    }
}

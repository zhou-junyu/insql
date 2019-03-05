namespace Insql
{
    public interface IInsql<TScope> : IInsql
        where TScope : class
    {
    }
}

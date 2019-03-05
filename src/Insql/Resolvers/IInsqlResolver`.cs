namespace Insql.Resolvers
{
    public interface IInsqlResolver<out T> : IInsqlResolver
        where T : class
    {
    }
}

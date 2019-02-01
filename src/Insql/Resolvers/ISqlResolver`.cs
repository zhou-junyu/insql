namespace Insql.Resolvers
{
    public interface ISqlResolver<out T> : ISqlResolver
        where T : class
    {
    }
}

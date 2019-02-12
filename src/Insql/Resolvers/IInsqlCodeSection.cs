namespace Insql.Resolvers
{
    public interface IInsqlCodeSection
    {
        string Id { get; }

        object Resolve(ResolveContext context);
    }
}

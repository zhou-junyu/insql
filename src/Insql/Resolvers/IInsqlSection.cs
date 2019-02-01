namespace Insql.Resolvers
{
    public interface IInsqlSection
    {
        string Id { get; }

        object Resolve(ResolveContext context);
    }
}

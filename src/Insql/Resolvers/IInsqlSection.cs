namespace Insql.Resolvers
{
    public interface IInsqlSection
    {
        string Id { get; }

        string Resolve(ResolveContext context);
    }
}

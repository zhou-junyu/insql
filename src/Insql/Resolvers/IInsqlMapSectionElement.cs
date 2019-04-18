namespace Insql.Resolvers
{
    public interface IInsqlMapSectionElement
    {
        string Name { get; }

        string Property { get; }

        bool Identity { get; }

        InsqlMapElementType ElementType { get; }
    }

    public enum InsqlMapElementType
    {
        None,
        Key,
        Column
    }
}

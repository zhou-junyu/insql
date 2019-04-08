namespace Insql.Resolvers
{
    public interface IInsqlMapSectionElement
    {
        string Name { get; }

        string To { get; }

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

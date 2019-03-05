using System.Reflection;

namespace Insql.Mappers
{
    public interface IInsqlPropertyMap
    {
        bool IsKey { get; }

        bool IsIdentity { get; }

        bool IsIgnored { get; }

        string ColumnName { get; }

        PropertyInfo PropertyInfo { get; }
    }
}

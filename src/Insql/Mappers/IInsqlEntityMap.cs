using System;
using System.Collections.Generic;

namespace Insql.Mappers
{
    public interface IInsqlEntityMap
    {
        Type EntityType { get; }

        string TableName { get; }

        IList<IInsqlPropertyMap> PropertyMaps { get; }
    }
}

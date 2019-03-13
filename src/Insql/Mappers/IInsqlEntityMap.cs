using System;
using System.Collections.Generic;

namespace Insql.Mappers
{
    public interface IInsqlEntityMap
    {
        Type EntityType { get; }

        string Table { get; }

        string Schema { get; }

        IList<IInsqlPropertyMap> Properties { get; }
    }
}

using System;
using System.Collections.Generic;

namespace Insql.Mappers
{
    public interface IInsqlModel
    {
        IEnumerable<IInsqlEntityMap> Maps { get; }

        IInsqlEntityMap FindMap(Type entityType);
    }
}

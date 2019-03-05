using System;
using System.Collections.Generic;

namespace Insql.Mappers
{
    public interface IInsqlModel
    {
        IInsqlEntityMap GetMap(Type entityType);

        IEnumerable<IInsqlEntityMap> GetMaps();
    }
}

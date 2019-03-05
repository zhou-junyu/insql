using System;
using System.Collections.Generic;

namespace Insql.Mappers
{
    public interface IInsqlEntityMapper
    {
        void Mapping(IDictionary<Type, IInsqlEntityMap> maps);
    }
}

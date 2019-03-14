using System;
using System.Collections.Generic;

namespace Insql.Mappers
{
    public interface IInsqlModelMapper
    {
        void Mapping(IDictionary<Type, IInsqlEntityMap> maps);
    }
}

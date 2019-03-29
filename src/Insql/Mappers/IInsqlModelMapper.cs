using System.Collections.Generic;

namespace Insql.Mappers
{
    public interface IInsqlModelMapper
    {
        void Mapping(IEnumerable<IInsqlEntityMap> maps);
    }
}

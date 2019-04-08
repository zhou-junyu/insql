using Dapper;
using System.Collections.Generic;

namespace Insql.Mappers
{
    internal class DapperModelMapper : IInsqlModelMapper
    {
        public void Mapping(IEnumerable<IInsqlEntityMap> maps)
        {
            foreach (var map in maps)
            {
                SqlMapper.SetTypeMap(map.EntityType, new DapperTypeMap(map));
            }
        }
    }
}

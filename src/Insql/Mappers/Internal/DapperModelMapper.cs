using Dapper;
using System;
using System.Collections.Generic;

namespace Insql.Mappers
{
    internal class DapperModelMapper : IInsqlModelMapper
    {
        public void Mapping(IDictionary<Type, IInsqlEntityMap> maps)
        {
            foreach (var map in maps)
            {
                SqlMapper.SetTypeMap(map.Key, new DapperTypeMap(map.Value));
            }
        }
    }
}

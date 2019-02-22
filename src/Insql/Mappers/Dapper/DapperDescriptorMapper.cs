using Dapper;
using Insql.Resolvers;
using System.Collections.Generic;

namespace Insql.Mappers
{
    internal class DapperDescriptorMapper : IInsqlDescriptorMapper
    {
        public void Mapping(IEnumerable<InsqlDescriptor> descriptors)
        {
            foreach (var descriptor in descriptors)
            {
                foreach (var map in descriptor.Maps.Values)
                {
                    SqlMapper.SetTypeMap(map.Type, new DapperTypeMap(map));
                }
            }
        }
    }
}

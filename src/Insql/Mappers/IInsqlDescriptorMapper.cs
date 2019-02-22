using Insql.Resolvers;
using System.Collections.Generic;

namespace Insql.Mappers
{
    public interface IInsqlDescriptorMapper
    {
        void Mapping(IEnumerable<InsqlDescriptor> descriptors);
    }
}

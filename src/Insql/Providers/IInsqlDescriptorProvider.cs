using Insql.Resolvers;
using System.Collections.Generic;

namespace Insql.Providers
{
    public interface IInsqlDescriptorProvider
    {
        IEnumerable<InsqlDescriptor> GetDescriptors();
    }
}

using Insql.Resolvers;
using System;
using System.Collections.Generic;

namespace Insql.Providers
{
    public interface IInsqlDescriptorLoader
    {
        IDictionary<Type, InsqlDescriptor> Load();
    }
}

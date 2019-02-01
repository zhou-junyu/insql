using System;

namespace Insql.Resolvers
{
    public interface ISqlResolverFactory
    {
        ISqlResolver GetResolver(Type type);
    }
}

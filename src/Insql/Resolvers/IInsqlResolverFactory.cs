using System;

namespace Insql.Resolvers
{
    public interface IInsqlResolverFactory
    {
        IInsqlResolver GetResolver(Type type);
    }
}

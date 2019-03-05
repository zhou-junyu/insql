using System;

namespace Insql.Resolvers
{
    public interface IInsqlResolverFactory
    {
        IInsqlResolver CreateResolver(Type scopeType);
    }
}

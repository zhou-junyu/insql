using System;

namespace Insql
{
    public interface IDbSessionFactory
    {
        IDbSession CreateSession(Type scopeType);
    }
}

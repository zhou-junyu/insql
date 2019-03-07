using System;

namespace Insql
{
    public interface IDbContextFactory
    {
        IDbContext CreateContext(Type contextType);
    }
}

using System;

namespace Insql
{
    public interface IInsqlFactory
    {
        IInsql Create(Type scopeType);
    }
}

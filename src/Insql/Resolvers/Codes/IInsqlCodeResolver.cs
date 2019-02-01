using System;
using System.Collections.Generic;

namespace Insql.Resolvers.Codes
{
    public interface IInsqlCodeResolver : IDisposable
    {
        object Resolve(Type type, string code, IDictionary<string, object> param);
    }
}

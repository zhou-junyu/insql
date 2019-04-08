using System;
using System.Collections.Generic;

namespace Insql.Resolvers
{
    public interface IInsqlResolveScripter : IDisposable
    {
        object Resolve(TypeCode type, string code, IDictionary<string, object> param);
    }
}

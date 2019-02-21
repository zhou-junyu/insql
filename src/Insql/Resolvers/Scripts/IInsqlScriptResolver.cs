using System;
using System.Collections.Generic;

namespace Insql.Resolvers.Scripts
{
    public interface IInsqlScriptResolver : IDisposable
    {
        object Resolve(TypeCode type, string code, IDictionary<string, object> param);
    }
}

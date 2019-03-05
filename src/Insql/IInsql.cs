using Insql.Mappers;
using System;

namespace Insql
{
    public interface IInsql : IDisposable
    {


        IDbSession Session { get; }

        IInsqlModel Model { get; }
    }
}

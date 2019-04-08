using System;
using System.Data;

namespace Insql
{
    public interface IDbSession : IDisposable
    {
        IDbConnection Connection { get; }

        IDbTransaction Transaction { get; }

        int? CommandTimeout { get; set; }

        void BeginTransaction();

        void BeginTransaction(IsolationLevel isolationLevel);

        void CommitTransaction();

        void RollbackTransaction();
    }
}

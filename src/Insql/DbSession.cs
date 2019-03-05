using System;
using System.Data;

namespace Insql
{
    public class DbSession : IDbSession
    {
        private readonly bool wasDisposed;
        private readonly bool originalClosed;

        private bool wasClosed;
        private bool isDisposed;

        public DbSession(IDbConnection connection, bool wasDisposed)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            this.Connection = connection;

            this.wasDisposed = wasDisposed;

            this.originalClosed = this.Connection.State == ConnectionState.Closed;
        }

        public IDbConnection Connection { get; }

        public IDbTransaction Transaction { get; private set; }

        public int? CommandTimeout { get; set; }

        public void BeginTransaction()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
            if (this.Transaction != null)
            {
                throw new Exception("The current transaction already exists.");
            }

            this.OpenTransaction();

            this.Transaction = this.Connection.BeginTransaction();
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
            if (this.Transaction != null)
            {
                throw new Exception("The current transaction already exists.");
            }

            this.OpenTransaction();

            this.Transaction = this.Connection.BeginTransaction(isolationLevel);
        }

        public void CommitTransaction()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
            if (this.Transaction == null)
            {
                throw new Exception("Current transaction does not exist.");
            }

            try
            {
                this.Transaction.Commit();
            }
            finally
            {
                this.CloseTransaction();
            }
        }

        public void RollbackTransaction()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
            if (this.Transaction == null)
            {
                throw new Exception("Current transaction does not exist.");
            }

            try
            {
                this.Transaction.Rollback();
            }
            finally
            {
                this.CloseTransaction();
            }
        }

        public void Dispose()
        {
            if (this.isDisposed)
            {
                return;
            }

            this.isDisposed = true;

            if (this.Transaction != null)
            {
                try
                {
                    this.Transaction.Rollback();
                }
                catch
                {
                }

                this.CloseTransaction();
            }

            if (this.originalClosed && this.Connection.State != ConnectionState.Closed)
            {
                this.Connection.Close();
            }

            if (this.wasDisposed)
            {
                this.Connection.Dispose();
            }
        }

        private void OpenTransaction()
        {
            if (this.Connection.State == ConnectionState.Closed)
            {
                this.wasClosed = true;

                this.Connection.Open();
            }
        }

        private void CloseTransaction()
        {
            try
            {
                this.Transaction.Dispose();
            }
            catch
            {
            }

            this.Transaction = null;

            if (this.wasClosed)
            {
                this.wasClosed = false;

                this.Connection.Close();
            }
        }
    }
}

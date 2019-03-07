using System;
using System.Data;

namespace Insql
{
    public class DbSession : IDbSession
    {
        private readonly bool isDisposable;
        private readonly bool isClosed;

        private bool isDisposed;
        private bool wasClosed;

        private IDbConnection connection;
        private IDbTransaction transaction;

        public DbSession(IDbConnection connection, bool disposable)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            this.connection = connection;

            this.isDisposable = disposable;

            this.isClosed = this.connection.State == ConnectionState.Closed;
        }

        public IDbConnection Connection => this.connection;

        public IDbTransaction Transaction => this.transaction;

        public int? CommandTimeout { get; set; }

        public void BeginTransaction()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
            if (this.transaction != null)
            {
                throw new Exception("The current transaction already exists.");
            }

            this.OpenTransaction();

            this.transaction = this.connection.BeginTransaction();
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
            if (this.transaction != null)
            {
                throw new Exception("The current transaction already exists.");
            }

            this.OpenTransaction();

            this.transaction = this.connection.BeginTransaction(isolationLevel);
        }

        public void CommitTransaction()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
            if (this.transaction == null)
            {
                throw new Exception("Current transaction does not exist.");
            }

            try
            {
                this.transaction.Commit();
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
            if (this.transaction == null)
            {
                throw new Exception("Current transaction does not exist.");
            }

            try
            {
                this.transaction.Rollback();
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

            if (this.transaction != null)
            {
                try
                {
                    this.transaction.Rollback();
                }
                catch
                {
                }

                this.CloseTransaction();
            }

            if (this.isClosed && this.connection.State != ConnectionState.Closed)
            {
                this.connection.Close();
            }

            if (this.isDisposable)
            {
                this.connection.Dispose();
            }
        }

        private void OpenTransaction()
        {
            if (this.connection.State == ConnectionState.Closed)
            {
                this.wasClosed = true;

                this.connection.Open();
            }
        }

        private void CloseTransaction()
        {
            try
            {
                this.transaction.Dispose();
            }
            catch
            {
            }

            this.transaction = null;

            if (this.wasClosed)
            {
                this.wasClosed = false;

                this.connection.Close();
            }
        }
    }
}

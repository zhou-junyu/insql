using System;
using System.Data;

namespace Insql
{
    internal class DbSession : IDbSession
    {
        private readonly bool isConnectionClosed;
        private readonly bool wasConnectionDisposed;

        private bool wasClosed;
        private bool isDisposed;

        public DbSession(IDbConnectionFactory connectionFactory, string connectionString)
        {
            if (connectionFactory == null)
            {
                throw new ArgumentNullException(nameof(connectionFactory));
            }
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            this.CurrentConnection = connectionFactory.CreateConnection();
            this.CurrentConnection.ConnectionString = connectionString;

            this.isConnectionClosed = this.CurrentConnection.State == ConnectionState.Closed;
            this.wasConnectionDisposed = true;
        }

        public DbSession(IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            this.CurrentConnection = connection;

            this.isConnectionClosed = this.CurrentConnection.State == ConnectionState.Closed;
        }

        public IDbConnection CurrentConnection { get; }

        public IDbTransaction CurrentTransaction { get; private set; }

        public int? CommandTimeout { get; set; }

        public void BeginTransaction()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
            if (this.CurrentTransaction != null)
            {
                throw new Exception("The current transaction already exists.");
            }

            this.OpenTransaction();

            this.CurrentTransaction = this.CurrentConnection.BeginTransaction();
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
            if (this.CurrentTransaction != null)
            {
                throw new Exception("The current transaction already exists.");
            }

            this.OpenTransaction();

            this.CurrentTransaction = this.CurrentConnection.BeginTransaction(isolationLevel);
        }

        public void CommitTransaction()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
            if (this.CurrentTransaction == null)
            {
                throw new Exception("Current transaction does not exist.");
            }

            try
            {
                this.CurrentTransaction.Commit();
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
            if (this.CurrentTransaction == null)
            {
                throw new Exception("Current transaction does not exist.");
            }

            try
            {
                this.CurrentTransaction.Rollback();
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

            if (this.CurrentTransaction != null)
            {
                try
                {
                    this.CurrentTransaction.Rollback();
                }
                catch
                {
                }

                this.CloseTransaction();
            }

            if (this.isConnectionClosed && this.CurrentConnection.State != ConnectionState.Closed)
            {
                this.CurrentConnection.Close();
            }

            if (this.wasConnectionDisposed)
            {
                this.CurrentConnection.Dispose();
            }
        }

        private void OpenTransaction()
        {
            if (this.CurrentConnection.State == ConnectionState.Closed)
            {
                this.wasClosed = true;

                this.CurrentConnection.Open();
            }
        }

        private void CloseTransaction()
        {
            try
            {
                this.CurrentTransaction.Dispose();
            }
            catch
            {
            }

            this.CurrentTransaction = null;

            if (this.wasClosed)
            {
                this.wasClosed = false;

                this.CurrentConnection.Close();
            }
        }
    }
}

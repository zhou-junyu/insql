using System;
using System.Data;

namespace Insql
{
    internal class DbSession : IDbSession
    {
        private readonly bool wasDisposed;
        private bool wasClosed;
        private bool isDisposed;

        public DbSession(IDbConnectionFactory connectionFactory, string connectionString, int? commandTimeout)
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
            this.CommandTimeout = commandTimeout;

            this.wasDisposed = true;
        }

        public DbSession(IDbConnection connection, int? commandTimeout)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            this.CurrentConnection = connection;
            this.CommandTimeout = commandTimeout;
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

            this.OpenConnection();

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

            this.OpenConnection();

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

            if (this.wasDisposed)
            {
                this.CurrentConnection.Dispose();
            }
        }

        private void OpenConnection()
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

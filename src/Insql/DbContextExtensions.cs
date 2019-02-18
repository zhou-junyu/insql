using System;
using System.Data;

namespace Insql
{
    public static partial class DbContextExtensions
    {
        public static void BeginTransaction(this DbContext dbContext)
        {
            dbContext.DbSession.BeginTransaction();
        }

        public static void BeginTransaction(this DbContext dbContext, IsolationLevel isolationLevel)
        {
            dbContext.DbSession.BeginTransaction(isolationLevel);
        }

        public static void CommitTransaction(this DbContext dbContext)
        {
            dbContext.DbSession.CommitTransaction();
        }

        public static void RollbackTransaction(this DbContext dbContext)
        {
            dbContext.DbSession.RollbackTransaction();
        }

        public static void DoWithTransaction(this DbContext dbContext, Action action)
        {
            dbContext.DbSession.BeginTransaction();

            ExecuteTransactionAction(dbContext, action);
        }

        public static void DoWithTransaction(this DbContext dbContext, Action action, IsolationLevel isolationLevel)
        {
            dbContext.DbSession.BeginTransaction(isolationLevel);

            ExecuteTransactionAction(dbContext, action);
        }

        public static T DoWithTransaction<T>(this DbContext dbContext, Func<T> action)
        {
            dbContext.DbSession.BeginTransaction();

            return ExecuteTransactionAction(dbContext, action);
        }

        public static T DoWithTransaction<T>(this DbContext dbContext, Func<T> action, IsolationLevel isolationLevel)
        {
            dbContext.DbSession.BeginTransaction(isolationLevel);

            return ExecuteTransactionAction(dbContext, action);
        }

        public static void DoWithOpen(this DbContext dbContext, Action action)
        {
            var wasClosed = false;

            try
            {
                if (dbContext.DbSession.CurrentConnection.State == ConnectionState.Closed)
                {
                    wasClosed = true;

                    dbContext.DbSession.CurrentConnection.Open();
                }

                action();
            }
            finally
            {
                if (wasClosed)
                {
                    dbContext.DbSession.CurrentConnection.Close();
                }
            }
        }

        public static T DoWithOpen<T>(this DbContext dbContext, Func<T> action)
        {
            var wasClosed = false;

            try
            {
                if (dbContext.DbSession.CurrentConnection.State == ConnectionState.Closed)
                {
                    wasClosed = true;

                    dbContext.DbSession.CurrentConnection.Open();
                }

                return action();
            }
            finally
            {
                if (wasClosed)
                {
                    dbContext.DbSession.CurrentConnection.Close();
                }
            }
        }

        static void ExecuteTransactionAction(DbContext dbContext, Action action)
        {
            try
            {
                action();

                dbContext.DbSession.CommitTransaction();
            }
            catch
            {
                if (dbContext.DbSession.CurrentTransaction != null)
                {
                    dbContext.DbSession.RollbackTransaction();
                }

                throw;
            }
        }

        static T ExecuteTransactionAction<T>(DbContext dbContext, Func<T> action)
        {
            try
            {
                var result = action();

                dbContext.DbSession.CommitTransaction();

                return result;
            }
            catch
            {
                if (dbContext.DbSession.CurrentTransaction != null)
                {
                    dbContext.DbSession.RollbackTransaction();
                }

                throw;
            }
        }
    }
}

using System;
using System.Data;

namespace Insql
{
    public static partial class DbContextExtensions
    {
        public static void BeginTransaction(this DbContext dbContext)
        {
            dbContext.Session.BeginTransaction();
        }

        public static void BeginTransaction(this DbContext dbContext, IsolationLevel isolationLevel)
        {
            dbContext.Session.BeginTransaction(isolationLevel);
        }

        public static void CommitTransaction(this DbContext dbContext)
        {
            dbContext.Session.CommitTransaction();
        }

        public static void RollbackTransaction(this DbContext dbContext)
        {
            dbContext.Session.RollbackTransaction();
        }

        public static void DoWithTransaction(this DbContext dbContext, Action action)
        {
            if (dbContext.Session.Transaction != null)
            {
                action();
            }
            else
            {
                dbContext.Session.BeginTransaction();

                ExecuteTransactionAction(dbContext, action);
            }
        }

        public static void DoWithTransaction(this DbContext dbContext, Action action, IsolationLevel isolationLevel)
        {
            if (dbContext.Session.Transaction != null)
            {
                action();
            }
            else
            {
                dbContext.Session.BeginTransaction(isolationLevel);

                ExecuteTransactionAction(dbContext, action);
            }
        }

        public static T DoWithTransaction<T>(this DbContext dbContext, Func<T> action)
        {
            if (dbContext.Session.Transaction != null)
            {
                return action();
            }
            else
            {
                dbContext.Session.BeginTransaction();

                return ExecuteTransactionAction(dbContext, action);
            }
        }

        public static T DoWithTransaction<T>(this DbContext dbContext, Func<T> action, IsolationLevel isolationLevel)
        {
            if (dbContext.Session.Transaction != null)
            {
                return action();
            }
            else
            {
                dbContext.Session.BeginTransaction(isolationLevel);

                return ExecuteTransactionAction(dbContext, action);
            }
        }

        public static void DoWithOpen(this DbContext dbContext, Action action)
        {
            var wasClosed = false;

            try
            {
                if (dbContext.Session.Connection.State == ConnectionState.Closed)
                {
                    wasClosed = true;

                    dbContext.Session.Connection.Open();
                }

                action();
            }
            finally
            {
                if (wasClosed)
                {
                    dbContext.Session.Connection.Close();
                }
            }
        }

        public static T DoWithOpen<T>(this DbContext dbContext, Func<T> action)
        {
            var wasClosed = false;

            try
            {
                if (dbContext.Session.Connection.State == ConnectionState.Closed)
                {
                    wasClosed = true;

                    dbContext.Session.Connection.Open();
                }

                return action();
            }
            finally
            {
                if (wasClosed)
                {
                    dbContext.Session.Connection.Close();
                }
            }
        }

        static void ExecuteTransactionAction(DbContext dbContext, Action action)
        {
            try
            {
                action();

                dbContext.Session.CommitTransaction();
            }
            catch
            {
                if (dbContext.Session.Transaction != null)
                {
                    dbContext.Session.RollbackTransaction();
                }

                throw;
            }
        }

        static T ExecuteTransactionAction<T>(DbContext dbContext, Func<T> action)
        {
            try
            {
                var result = action();

                dbContext.Session.CommitTransaction();

                return result;
            }
            catch
            {
                if (dbContext.Session.Transaction != null)
                {
                    dbContext.Session.RollbackTransaction();
                }

                throw;
            }
        }
    }
}

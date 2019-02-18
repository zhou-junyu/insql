using System;
using System.Data;

namespace Insql
{
    public static partial class DbSessionExtensions
    {
        public static void DoWithTransaction(this IDbSession dbSession, Action action)
        {
            if (dbSession.CurrentTransaction != null)
            {
                action();
            }
            else
            {
                dbSession.BeginTransaction();

                ExecuteTransactionAction(dbSession, action);
            }
        }

        public static void DoWithTransaction(this IDbSession dbSession, Action action, IsolationLevel isolationLevel)
        {
            if (dbSession.CurrentTransaction != null)
            {
                action();
            }
            else
            {
                dbSession.BeginTransaction(isolationLevel);

                ExecuteTransactionAction(dbSession, action);
            }
        }

        public static T DoWithTransaction<T>(this IDbSession dbSession, Func<T> action)
        {
            if (dbSession.CurrentTransaction != null)
            {
                return action();
            }
            else
            {
                dbSession.BeginTransaction();

                return ExecuteTransactionAction(dbSession, action);
            }
        }

        public static T DoWithTransaction<T>(this IDbSession dbSession, Func<T> action, IsolationLevel isolationLevel)
        {
            if (dbSession.CurrentTransaction != null)
            {
                return action();
            }
            else
            {
                dbSession.BeginTransaction(isolationLevel);

                return ExecuteTransactionAction(dbSession, action);
            }
        }

        public static void DoWithOpen(this IDbSession dbSession, Action action)
        {
            var wasClosed = false;

            try
            {
                if (dbSession.CurrentConnection.State == ConnectionState.Closed)
                {
                    wasClosed = true;

                    dbSession.CurrentConnection.Open();
                }

                action();
            }
            finally
            {
                if (wasClosed)
                {
                    dbSession.CurrentConnection.Close();
                }
            }
        }

        public static T DoWithOpen<T>(this IDbSession dbSession, Func<T> action)
        {
            var wasClosed = false;

            try
            {
                if (dbSession.CurrentConnection.State == ConnectionState.Closed)
                {
                    wasClosed = true;

                    dbSession.CurrentConnection.Open();
                }

                return action();
            }
            finally
            {
                if (wasClosed)
                {
                    dbSession.CurrentConnection.Close();
                }
            }
        }

        static void ExecuteTransactionAction(IDbSession dbSession, Action action)
        {
            try
            {
                action();

                dbSession.CommitTransaction();
            }
            catch
            {
                if (dbSession.CurrentTransaction != null)
                {
                    dbSession.RollbackTransaction();
                }

                throw;
            }
        }

        static T ExecuteTransactionAction<T>(IDbSession dbSession, Func<T> action)
        {
            try
            {
                var result = action();

                dbSession.CommitTransaction();

                return result;
            }
            catch
            {
                if (dbSession.CurrentTransaction != null)
                {
                    dbSession.RollbackTransaction();
                }

                throw;
            }
        }
    }
}

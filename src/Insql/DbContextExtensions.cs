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

            ExecuteAction(dbContext, action);
        }

        public static void DoWithTransaction(this DbContext dbContext, Action action, IsolationLevel isolationLevel)
        {
            dbContext.DbSession.BeginTransaction(isolationLevel);

            ExecuteAction(dbContext, action);
        }

        public static T DoWithTransaction<T>(this DbContext dbContext, Func<T> action)
        {
            dbContext.DbSession.BeginTransaction();

            return ExecuteAction(dbContext, action);
        }

        public static T DoWithTransaction<T>(this DbContext dbContext, Func<T> action, IsolationLevel isolationLevel)
        {
            dbContext.DbSession.BeginTransaction(isolationLevel);

            return ExecuteAction(dbContext, action);
        }

        static void ExecuteAction(DbContext dbContext, Action action)
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

        static T ExecuteAction<T>(DbContext dbContext, Func<T> action)
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

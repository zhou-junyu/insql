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
            dbContext.DbSession.DoWithTransaction(action);
        }

        public static void DoWithTransaction(this DbContext dbContext, Action action, IsolationLevel isolationLevel)
        {
            dbContext.DbSession.DoWithTransaction(action, isolationLevel);
        }

        public static T DoWithTransaction<T>(this DbContext dbContext, Func<T> action)
        {
            return dbContext.DbSession.DoWithTransaction<T>(action);
        }

        public static T DoWithTransaction<T>(this DbContext dbContext, Func<T> action, IsolationLevel isolationLevel)
        {
            return dbContext.DbSession.DoWithTransaction<T>(action, isolationLevel);
        }

        public static void DoWithOpen(this DbContext dbContext, Action action)
        {
            dbContext.DbSession.DoWithOpen(action);
        }

        public static T DoWithOpen<T>(this DbContext dbContext, Func<T> action)
        {
            return dbContext.DbSession.DoWithOpen<T>(action);
        }
    }
}

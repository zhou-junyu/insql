using Dapper;
using Insql.Mappers;
using Insql.Resolvers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Insql
{
    public class DbContext : IDisposable
    {
        private IDbSession session;

        private readonly IInsqlModel insqlModel;
        private readonly IInsqlResolver insqlResolver;
        private readonly IDbSessionFactory sessionFactory;

        public Type Type => throw new NotImplementedException();

        public IInsqlModel Model => throw new NotImplementedException();

        public IDbSession Session => throw new NotImplementedException();

        public IDbDialect Dialect => throw new NotImplementedException();

        public DbContext(DbContextOptions options)
        {

        }

        public void Dispose()
        {
            if (this.session != null)
            {
                this.session.Dispose();
            }
        }

        private IDbSession GetSession()
        {
            //todo 
            //if (this.session == null)
            //{
            //    lock (this.syncLock)
            //    {
            //        if (this.session == null)
            //        {
            //            this.session = this.sessionFactory.CreateSession(this.Type);
            //        }
            //    }
            //}

            return this.session;
        }

        public int Execute(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return this.Session.Connection.Execute(resolveResult.Sql, resolveResult.Param, this.Session.Transaction, this.Session.CommandTimeout);
        }

        public Task<int> ExecuteAsync(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return this.Session.Connection.ExecuteAsync(resolveResult.Sql, resolveResult.Param, this.Session.Transaction, this.Session.CommandTimeout);
        }

        public IDataReader ExecuteReader(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return this.Session.Connection.ExecuteReader(resolveResult.Sql, resolveResult.Param, this.Session.Transaction, this.Session.CommandTimeout);
        }

        public Task<IDataReader> ExecuteReaderAsync(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return this.Session.Connection.ExecuteReaderAsync(resolveResult.Sql, resolveResult.Param, this.Session.Transaction, this.Session.CommandTimeout);
        }

        public object ExecuteScalar(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return this.Session.Connection.ExecuteScalar(resolveResult.Sql, resolveResult.Param, this.Session.Transaction, this.Session.CommandTimeout);
        }

        public T ExecuteScalar<T>(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return this.Session.Connection.ExecuteScalar<T>(resolveResult.Sql, resolveResult.Param, this.Session.Transaction, this.Session.CommandTimeout);
        }

        public Task<object> ExecuteScalarAsync(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return this.Session.Connection.ExecuteScalarAsync(resolveResult.Sql, resolveResult.Param, this.Session.Transaction, this.Session.CommandTimeout);
        }

        public Task<T> ExecuteScalarAsync<T>(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return this.Session.Connection.ExecuteScalarAsync<T>(resolveResult.Sql, resolveResult.Param, this.Session.Transaction, this.Session.CommandTimeout);
        }

        public IEnumerable<T> Query<T>(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return this.Session.Connection.Query<T>(resolveResult.Sql, resolveResult.Param, this.Session.Transaction, true, this.Session.CommandTimeout);
        }

        public IEnumerable<object> Query(Type type, string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return this.Session.Connection.Query(type, resolveResult.Sql, resolveResult.Param, this.Session.Transaction, true, this.Session.CommandTimeout);
        }

        public IEnumerable<dynamic> Query(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return this.Session.Connection.Query(resolveResult.Sql, resolveResult.Param, this.Session.Transaction, true, this.Session.CommandTimeout);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return this.Session.Connection.QueryAsync<T>(resolveResult.Sql, resolveResult.Param, this.Session.Transaction, this.Session.CommandTimeout);
        }

        public Task<IEnumerable<object>> QueryAsync(Type type, string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return this.Session.Connection.QueryAsync(type, resolveResult.Sql, resolveResult.Param, this.Session.Transaction, this.Session.CommandTimeout);
        }

        public Task<IEnumerable<dynamic>> QueryAsync(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return this.Session.Connection.QueryAsync(resolveResult.Sql, resolveResult.Param, this.Session.Transaction, this.Session.CommandTimeout);
        }

        public virtual ResolveResult Resolve(string sqlId, object sqlParam = null)
        {
            return this.insqlResolver.Resolve(sqlId, sqlParam);
        }

        protected virtual void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected virtual void OnModelCreating(InsqlModelBuilder modelBuilder)
        {
            
        }
    }
}

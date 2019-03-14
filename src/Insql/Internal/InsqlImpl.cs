using Dapper;
using Insql.Mappers;
using Insql.Resolvers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Insql
{
    internal class InsqlImpl : IInsql
    {
        private readonly object syncLock = new object();

        private readonly IInsqlModel insqlModel;
        private readonly IInsqlResolver insqlResolver;
        private readonly IDbSessionFactory sessionFactory;

        private IDbSession dbSession;

        public InsqlImpl(Type contextType, IInsqlModel insqlModel, IInsqlResolver insqlResolver, IDbSessionFactory sessionFactory)
        {
            this.Type = contextType;
            this.insqlModel = insqlModel;
            this.insqlResolver = insqlResolver;
            this.sessionFactory = sessionFactory;
        }

        public Type Type { get; }

        public IInsqlModel Model => this.insqlModel;

        public IDbSession Session
        {
            get
            {
                return this.GetSession();
            }
        }

        public IDbDialect Dialect => throw new NotImplementedException();

        public void Dispose()
        {
            if (this.dbSession != null)
            {
                this.dbSession.Dispose();
            }
        }

        private IDbSession GetSession()
        {
            if (this.dbSession == null)
            {
                lock (this.syncLock)
                {
                    if (this.dbSession == null)
                    {
                        this.dbSession = this.sessionFactory.CreateSession(this.Type);
                    }
                }
            }

            return this.dbSession;
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

        public ResolveResult Resolve(string sqlId, object sqlParam = null)
        {
            return this.insqlResolver.Resolve(sqlId, sqlParam);
        }
    }
}

using Insql.Mappers;
using Insql.Resolvers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Insql
{
    internal class InsqlImpl<TContext> : IInsql<TContext>
        where TContext : class
    {
        private readonly IInsql insql;

        public InsqlImpl(IInsqlFactory factory)
        {
            this.insql = factory.Create(typeof(TContext));
        }

        public Type Type => this.insql.Type;

        public IInsqlModel Model => this.insql.Model;

        public IDbSession Session => this.insql.Session;

        public IDbDialect Dialect => this.insql.Dialect;

        public void Dispose()
        {
            this.insql.Dispose();
        }

        public int Execute(string sqlId, object sqlParam = null)
        {
            return this.insql.Execute(sqlId, sqlParam);
        }

        public Task<int> ExecuteAsync(string sqlId, object sqlParam = null)
        {
            return this.insql.ExecuteAsync(sqlId, sqlParam);
        }

        public IDataReader ExecuteReader(string sqlId, object sqlParam = null)
        {
            return this.insql.ExecuteReader(sqlId, sqlParam);
        }

        public Task<IDataReader> ExecuteReaderAsync(string sqlId, object sqlParam = null)
        {
            return this.insql.ExecuteReaderAsync(sqlId, sqlParam);
        }

        public object ExecuteScalar(string sqlId, object sqlParam = null)
        {
            return this.insql.ExecuteScalar(sqlId, sqlParam);
        }

        public T ExecuteScalar<T>(string sqlId, object sqlParam = null)
        {
            return this.insql.ExecuteScalar<T>(sqlId, sqlParam);
        }

        public Task<object> ExecuteScalarAsync(string sqlId, object sqlParam = null)
        {
            return this.insql.ExecuteScalarAsync(sqlId, sqlParam);
        }

        public Task<T> ExecuteScalarAsync<T>(string sqlId, object sqlParam = null)
        {
            return this.insql.ExecuteScalarAsync<T>(sqlId, sqlParam);
        }

        public IEnumerable<T> Query<T>(string sqlId, object sqlParam = null)
        {
            return this.insql.Query<T>(sqlId, sqlParam);
        }

        public IEnumerable<object> Query(Type type, string sqlId, object sqlParam = null)
        {
            return this.insql.Query(type, sqlId, sqlParam);
        }

        public IEnumerable<dynamic> Query(string sqlId, object sqlParam = null)
        {
            return this.insql.Query(sqlId, sqlParam);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string sqlId, object sqlParam = null)
        {
            return this.insql.QueryAsync<T>(sqlId, sqlParam);
        }

        public Task<IEnumerable<object>> QueryAsync(Type type, string sqlId, object sqlParam = null)
        {
            return this.insql.QueryAsync(type, sqlId, sqlParam);
        }

        public Task<IEnumerable<dynamic>> QueryAsync(string sqlId, object sqlParam = null)
        {
            return this.insql.QueryAsync(sqlId, sqlParam);
        }

        public ResolveResult Resolve(string sqlId, object sqlParam = null)
        {
            return this.insql.Resolve(sqlId, sqlParam);
        }
    }
}

using Dapper;
using Insql.Resolvers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Insql
{
    public class DbContext : IDisposable
    {
        public virtual IDbSession DbSession { get; }

        protected virtual ISqlResolver SqlResolver { get; }

        private readonly IDictionary<string, string> sqlResolverEnvironment;

        public DbContext(DbContextOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this.OnConfiguring(options);

            if (options.SessionFactory == null)
            {
                throw new ArgumentNullException(nameof(options.SessionFactory));
            }
            if (options.SqlResolver == null)
            {
                throw new ArgumentNullException(nameof(options.SqlResolver));
            }

            this.DbSession = options.SessionFactory.CreateSession();

            this.SqlResolver = options.SqlResolver;
            this.sqlResolverEnvironment = options.SqlResolverEnvironment;
        }

        protected virtual void OnConfiguring(DbContextOptions options)
        {
        }

        public IEnumerable<T> Query<T>(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return this.DbSession.CurrentConnection.Query<T>(resolveResult.Sql, resolveResult.Param, this.DbSession.CurrentTransaction, true, this.DbSession.CommandTimeout);
        }

        public IEnumerable<object> Query<T>(Type type, string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return this.DbSession.CurrentConnection.Query(type, resolveResult.Sql, resolveResult.Param, this.DbSession.CurrentTransaction, true, this.DbSession.CommandTimeout);
        }

        public IEnumerable<dynamic> Query(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return this.DbSession.CurrentConnection.Query(resolveResult.Sql, resolveResult.Param, this.DbSession.CurrentTransaction, true, this.DbSession.CommandTimeout);
        }

        public IMultipleDataReader QueryMultiple(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            var gridReader = this.DbSession.CurrentConnection.QueryMultiple(resolveResult.Sql, resolveResult.Param, this.DbSession.CurrentTransaction, this.DbSession.CommandTimeout);

            return new GridMultipleDataReader(gridReader);
        }

        public int Execute(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return this.DbSession.CurrentConnection.Execute(resolveResult.Sql, resolveResult.Param, this.DbSession.CurrentTransaction, this.DbSession.CommandTimeout);
        }

        public object ExecuteScalar(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return this.DbSession.CurrentConnection.ExecuteScalar(resolveResult.Sql, resolveResult.Param, this.DbSession.CurrentTransaction, this.DbSession.CommandTimeout);
        }

        public T ExecuteScalar<T>(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return this.DbSession.CurrentConnection.ExecuteScalar<T>(resolveResult.Sql, resolveResult.Param, this.DbSession.CurrentTransaction, this.DbSession.CommandTimeout);
        }

        public IDataReader ExecuteReader(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return this.DbSession.CurrentConnection.ExecuteReader(resolveResult.Sql, resolveResult.Param, this.DbSession.CurrentTransaction, this.DbSession.CommandTimeout);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return await this.DbSession.CurrentConnection.QueryAsync<T>(resolveResult.Sql, resolveResult.Param, this.DbSession.CurrentTransaction, this.DbSession.CommandTimeout);
        }

        public async Task<IEnumerable<object>> QueryAsync<T>(Type type, string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return await this.DbSession.CurrentConnection.QueryAsync(type, resolveResult.Sql, resolveResult.Param, this.DbSession.CurrentTransaction, this.DbSession.CommandTimeout);
        }

        public async Task<IEnumerable<dynamic>> QueryAsync(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return await this.DbSession.CurrentConnection.QueryAsync(resolveResult.Sql, resolveResult.Param, this.DbSession.CurrentTransaction, this.DbSession.CommandTimeout);
        }

        public async Task<IMultipleDataReader> QueryMultipleAsync(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            var gridReader = await this.DbSession.CurrentConnection.QueryMultipleAsync(resolveResult.Sql, resolveResult.Param, this.DbSession.CurrentTransaction, this.DbSession.CommandTimeout);

            return new GridMultipleDataReader(gridReader);
        }

        public async Task<int> ExecuteAsync(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return await this.DbSession.CurrentConnection.ExecuteAsync(resolveResult.Sql, resolveResult.Param, this.DbSession.CurrentTransaction, this.DbSession.CommandTimeout);
        }

        public async Task<object> ExecuteScalarAsync(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return await this.DbSession.CurrentConnection.ExecuteScalarAsync(resolveResult.Sql, resolveResult.Param, this.DbSession.CurrentTransaction, this.DbSession.CommandTimeout);
        }

        public async Task<T> ExecuteScalarAsync<T>(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return await this.DbSession.CurrentConnection.ExecuteScalarAsync<T>(resolveResult.Sql, resolveResult.Param, this.DbSession.CurrentTransaction, this.DbSession.CommandTimeout);
        }

        public async Task<IDataReader> ExecuteReaderAsync(string sqlId, object sqlParam = null)
        {
            var resolveResult = this.Resolve(sqlId, sqlParam);

            return await this.DbSession.CurrentConnection.ExecuteReaderAsync(resolveResult.Sql, resolveResult.Param, this.DbSession.CurrentTransaction, this.DbSession.CommandTimeout);
        }

        public ResolveResult Resolve(string sqlId, object sqlParam = null)
        {
            return this.SqlResolver.Resolve(sqlId, sqlParam, new Dictionary<string, string>(this.sqlResolverEnvironment));
        }

        public void Dispose()
        {
            this.DbSession.Dispose();
        }
    }
}

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
        private readonly object syncLock = new object();

        private readonly DbContextOptions contextOptions;

        private IDbSession dbSession;

        private bool isDisposed;

        public virtual IDbSession DbSession
        {
            get
            {
                return this.GetSession();
            }
        }

        public DbContext(DbContextOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this.ConfigureOptions(options);

            if (options.SqlResolver == null)
            {
                throw new ArgumentNullException(nameof(options.SqlResolver));
            }
            if (options.SessionFactory == null)
            {
                throw new ArgumentNullException(nameof(options.SessionFactory));
            }

            this.contextOptions = options;
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

        public virtual ResolveResult Resolve(string sqlId, object sqlParam = null)
        {
            return this.contextOptions.SqlResolver.Resolve(this.DbSession.DbType, sqlId, sqlParam);
        }

        private void ConfigureOptions(DbContextOptions options)
        {
            if (!options.IsConfigured)
            {
                var needConfigure = false;

                lock (this.syncLock)
                {
                    if (!options.IsConfigured)
                    {
                        options.IsConfigured = true;

                        needConfigure = true;
                    }
                }

                if (needConfigure)
                {
                    this.OnConfiguring(options);
                }
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
                        this.dbSession = this.contextOptions.SessionFactory.CreateSession();
                    }
                }
            }

            return this.dbSession;
        }

        public void Dispose()
        {
            if (this.isDisposed)
            {
                return;
            }

            this.isDisposed = true;

            if (this.dbSession != null)
            {
                this.dbSession.Dispose();
            }
        }
    }
}

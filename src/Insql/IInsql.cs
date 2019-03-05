using Insql.Mappers;
using Insql.Resolvers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Insql
{
    public interface IInsql : IDisposable
    {
        Type Type { get; }

        IInsqlModel Model { get; }

        IDbSession Session { get; }

        ResolveResult Resolve(string sqlId, object sqlParam = null);

        IEnumerable<T> Query<T>(string sqlId, object sqlParam = null);

        IEnumerable<object> Query(Type type, string sqlId, object sqlParam = null);

        IEnumerable<dynamic> Query(string sqlId, object sqlParam = null);

        int Execute(string sqlId, object sqlParam = null);

        object ExecuteScalar(string sqlId, object sqlParam = null);

        T ExecuteScalar<T>(string sqlId, object sqlParam = null);

        IDataReader ExecuteReader(string sqlId, object sqlParam = null);


        Task<IEnumerable<T>> QueryAsync<T>(string sqlId, object sqlParam = null);

        Task<IEnumerable<object>> QueryAsync(Type type, string sqlId, object sqlParam = null);

        Task<IEnumerable<dynamic>> QueryAsync(string sqlId, object sqlParam = null);

        Task<int> ExecuteAsync(string sqlId, object sqlParam = null);

        Task<object> ExecuteScalarAsync(string sqlId, object sqlParam = null);

        Task<T> ExecuteScalarAsync<T>(string sqlId, object sqlParam = null);

        Task<IDataReader> ExecuteReaderAsync(string sqlId, object sqlParam = null);
    }
}

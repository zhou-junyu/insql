using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insql
{
    [Obsolete("This interface will be removed in the new version")]
    public interface IMultipleDataReader : IDisposable
    {
        IEnumerable<dynamic> Read();

        IEnumerable<object> Read(Type type);

        IEnumerable<T> Read<T>();

        Task<IEnumerable<dynamic>> ReadAsync();

        Task<IEnumerable<object>> ReadAsync(Type type);

        Task<IEnumerable<T>> ReadAsync<T>();
    }

    [Obsolete("This class will be removed in the new version")]
    public class GridMultipleDataReader : IMultipleDataReader
    {
        private readonly SqlMapper.GridReader reader;

        public GridMultipleDataReader(SqlMapper.GridReader reader)
        {
            this.reader = reader;
        }

        public void Dispose()
        {
            this.reader.Dispose();
        }

        public IEnumerable<T> Read<T>()
        {
            return this.reader.Read<T>();
        }

        public IEnumerable<dynamic> Read()
        {
            return this.reader.Read();
        }

        public IEnumerable<object> Read(Type type)
        {
            return this.reader.Read(type);
        }

        public Task<IEnumerable<T>> ReadAsync<T>()
        {
            return this.reader.ReadAsync<T>();
        }

        public Task<IEnumerable<dynamic>> ReadAsync()
        {
            return this.reader.ReadAsync();
        }

        public Task<IEnumerable<object>> ReadAsync(Type type)
        {
            return this.reader.ReadAsync(type);
        }
    }
}

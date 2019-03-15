using System;

namespace Insql
{
    public class DbContextOptionsBuilder : IInsqlOptionsBuilder
    {
        private readonly DbContextOptions options;

        public DbContextOptionsBuilder(Type type)
        {
            var optionsType = typeof(DbContextOptions<>).MakeGenericType(type);

            this.options = (DbContextOptions)Activator.CreateInstance(optionsType);
        }

        public IInsqlOptions Options => this.options;

        public Type Type => this.options.Type;

        public IInsqlOptionsBuilder UseExtension<TExtension>(TExtension extension) where TExtension : class, IInsqlOptionsExtension
        {
            this.options.WithExtension<TExtension>(extension);

            return this;
        }
    }
}

using System;

namespace Insql
{
    public class DbContextOptionsBuilder
    {
        private readonly DbContextOptions options;

        public DbContextOptionsBuilder(Type contextType)
        {
            var optionsType = typeof(DbContextOptions<>).MakeGenericType(contextType);

            this.options = (DbContextOptions)Activator.CreateInstance(optionsType);
        }

        public DbContextOptionsBuilder(DbContextOptions options)
        {
            this.options = options;
        }

        public Type ContextType => this.options.ContextType;

        public DbContextOptions Options => this.options;
    }
}

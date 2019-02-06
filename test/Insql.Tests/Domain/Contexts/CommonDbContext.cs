using System;

namespace Insql.Tests.Domain.Contexts
{
    public class CommonDbContext<TInsql> : DbContext where TInsql : class
    {
        public CommonDbContext(CommonDbContextOptions<TInsql> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptions options)
        {
            options.UseSqlResolver<TInsql>();

            options.UseSqlite("Data Source= ./insql.tests.db");
        }
    }

    public class CommonDbContextOptions<TInsql> : DbContextOptions<CommonDbContext<TInsql>> where TInsql : class
    {
        public CommonDbContextOptions(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}

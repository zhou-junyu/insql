using System;

namespace Insql.Tests.Contexts
{
    public class CommonDbContextOptions<T> : DbContextOptions<CommonDbContext<T>> where T : class
    {
        public CommonDbContextOptions(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }
    }
}

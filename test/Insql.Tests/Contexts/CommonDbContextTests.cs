using Dapper;
using Insql.Tests.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace Insql.Tests.Contexts
{
    public class CommonDbContextTests : TestsBase
    {
        public CommonDbContextTests()
        {
            using (var dbContext = this.GlobalServiceProvider.GetRequiredService<CommonDbContext<CommonDbContextTestsInfo>>())
            {
                dbContext.Session.Connection.Execute(" CREATE TABLE IF NOT EXISTS tests_info (id  INTEGER PRIMARY KEY AUTOINCREMENT,name TEXT ,type INTEGER,create_time DATETIME NOT NULL);  ");
            }
        }

        protected override void GlobalServiceProviderConfigure(IInsqlBuilder builder)
        {
            builder.Services.AddSingleton(typeof(CommonDbContextOptions<>));
            builder.Services.AddScoped(typeof(CommonDbContext<>));
        }

        [Fact]
        public void Insert()
        {
            using (var dbContext = this.GlobalServiceProvider.GetRequiredService<CommonDbContext<CommonDbContextTestsInfo>>())
            {
                var testsInfo = new CommonDbContextTestsInfo
                {
                    Name = Guid.NewGuid().ToString(),
                    Type = InfoType.Black,
                    CreateTime = DateTime.Now
                };

                var id = dbContext.ExecuteScalar<int>(nameof(Insert), testsInfo);

                Assert.True(id > 0);
            }
        }

        [Fact]
        public void SelectById()
        {
            using (var dbContext = this.GlobalServiceProvider.GetRequiredService<CommonDbContext<CommonDbContextTestsInfo>>())
            {
                var testsInfo = new CommonDbContextTestsInfo
                {
                    Name = Guid.NewGuid().ToString(),
                    Type = InfoType.Black,
                    CreateTime = DateTime.Now
                };

                var id = dbContext.ExecuteScalar<int>(nameof(Insert), testsInfo);

                var selectInfo = dbContext.Query<DbContextTestInfo>(nameof(SelectById), new { id }).SingleOrDefault();

                Assert.NotNull(selectInfo);
                Assert.Equal(selectInfo.Name, testsInfo.Name);
            }
        }
    }
}

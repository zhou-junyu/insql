using Dapper;
using Insql.Tests.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Insql.Tests.Contexts
{
    public class SqliteDbContextTests : TestsBase
    {
        public SqliteDbContextTests()
        {
            using (var dbContext = this.GlobalServiceProvider.GetRequiredService<SqliteDbContext>())
            {
                dbContext.Session.Connection.Execute(" CREATE TABLE IF NOT EXISTS tests_info (id  INTEGER PRIMARY KEY AUTOINCREMENT,name TEXT ,type INTEGER,create_time DATETIME NOT NULL);  ");
            }
        }

        protected override void GlobalServiceProviderConfigure(IInsqlBuilder builder)
        {
            builder.Services.AddDbContext<SqliteDbContext>(options =>
            {
                options.UseSqlite("Data Source= ./insql.tests.db");
            });
        }

        [Fact]
        public void Insert()
        {
            using (var dbContext = this.GlobalServiceProvider.GetRequiredService<SqliteDbContext>())
            {
                var testsInfo = new DbContextTestInfo
                {
                    Name = Guid.NewGuid().ToString(),
                    Type = InfoType.Black,
                    CreateTime = DateTime.Now
                };

                dbContext.Insert(testsInfo);

                Assert.True(testsInfo.Id > 0);
            }
        }

        [Fact]
        public void Select()
        {
            using (var dbContext = this.GlobalServiceProvider.GetRequiredService<SqliteDbContext>())
            {
                var testsInfo = new DbContextTestInfo
                {
                    Name = Guid.NewGuid().ToString(),
                    Type = InfoType.Black,
                    CreateTime = DateTime.Now
                };

                dbContext.Insert(testsInfo);

                var selectInfo = dbContext.SelectById(testsInfo.Id);

                Assert.NotNull(selectInfo);
                Assert.Equal(selectInfo.Name, testsInfo.Name);
            }
        }
    }
}

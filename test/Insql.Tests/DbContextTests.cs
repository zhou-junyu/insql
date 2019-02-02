using Dapper;
using Insql.Tests.Domain.Contexts;
using Insql.Tests.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace Insql.Tests
{
    public class DbContextTests : IDisposable
    {
        private readonly IServiceCollection serviceCollection;
        private readonly IServiceProvider serviceProvider;

        public DbContextTests()
        {
            this.serviceCollection = new ServiceCollection();

            this.serviceCollection.AddInsql();

            this.serviceCollection.AddInsqlDbContext<UserDbContext>(optinos =>
            {
                optinos.UserSqlite("Data Source= ./insql.tests.db");
            });

            this.serviceCollection.AddScoped<IUserService, UserService>();

            this.serviceProvider = this.serviceCollection.BuildServiceProvider();

            this.Init();
        }

        public void Dispose()
        {
            if (this.serviceProvider is IDisposable dis)
            {
                dis.Dispose();
            }
        }

        private void Init()
        {
            using (var scopeProvider = this.serviceProvider.CreateScope())
            {
                using (var dbContext = scopeProvider.ServiceProvider.GetRequiredService<UserDbContext>())
                {
                    dbContext.DbSession.CurrentConnection.Execute(" CREATE TABLE IF NOT EXISTS user_info (user_id  INTEGER PRIMARY KEY AUTOINCREMENT,user_name TEXT ,user_gender INTEGER);  ");
                }
            }
        }

        [Fact]
        public void InsertUser()
        {
            using (var scopeProvider = this.serviceProvider.CreateScope())
            {
                var userService = scopeProvider.ServiceProvider.GetRequiredService<IUserService>();

                var userInfo = new Domain.Models.UserInfo
                {
                    UserName = new Random().Next().ToString(),
                    UserGender = Domain.Models.UserGender.W
                };

                userService.InsertUser(userInfo);

                Assert.True(userInfo.UserId > 0);
            }
        }

        [Fact(Timeout = 500)]
        public void GetWUserList()
        {
            using (var scopeProvider = this.serviceProvider.CreateScope())
            {
                var userService = scopeProvider.ServiceProvider.GetRequiredService<IUserService>();

                var wlist = userService.GetUserList(null, Domain.Models.UserGender.W).ToList();

                Assert.True(wlist.All(o => o.UserGender == Domain.Models.UserGender.W));
            }
        }
    }
}

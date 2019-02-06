using Dapper;
using Insql.Tests.Domain.Contexts;
using Insql.Tests.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace Insql.Tests
{
    public class CommonDbContextTests : IDisposable
    {
        private readonly IServiceCollection serviceCollection;
        private readonly IServiceProvider serviceProvider;

        public CommonDbContextTests()
        {
            this.serviceCollection = new ServiceCollection();

            this.serviceCollection.AddInsql();

            this.serviceCollection.AddScoped(typeof(CommonDbContextOptions<>));
            this.serviceCollection.AddScoped(typeof(CommonDbContext<>));

            this.serviceCollection.AddScoped<ICommonUserService, CommonUserService>();

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
                using (var dbContext = scopeProvider.ServiceProvider.GetRequiredService<CommonDbContext<CommonUserService>>())
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
                var userService = scopeProvider.ServiceProvider.GetRequiredService<ICommonUserService>();

                var userInfo = new Domain.Models.UserInfo
                {
                    UserName = new Random().Next().ToString(),
                    UserGender = Domain.Models.UserGender.W
                };

                userService.InsertUser(userInfo);

                Assert.True(userInfo.UserId > 0);
            }
        }

        [Fact]
        public void InsertUpdateGetUser()
        {
            using (var scopeProvider = this.serviceProvider.CreateScope())
            {
                var userService = scopeProvider.ServiceProvider.GetRequiredService<ICommonUserService>();

                var userInfo = new Domain.Models.UserInfo
                {
                    UserName = new Random().Next().ToString(),
                    UserGender = Domain.Models.UserGender.W
                };

                userService.InsertUser(userInfo);

                Assert.True(userInfo.UserId > 0);

                userInfo.UserGender = null; //set null

                userService.UpdateUserSelective(userInfo);

                var updatedUserInfo = userService.GetUser(userInfo.UserId);

                Assert.NotNull(updatedUserInfo);
                Assert.True(updatedUserInfo.UserGender == Domain.Models.UserGender.W);
            }
        }

        [Fact]
        public void InsertDeleteGetUser()
        {
            using (var scopeProvider = this.serviceProvider.CreateScope())
            {
                var userService = scopeProvider.ServiceProvider.GetRequiredService<ICommonUserService>();

                var userInfo = new Domain.Models.UserInfo
                {
                    UserName = new Random().Next().ToString(),
                    UserGender = Domain.Models.UserGender.W
                };

                userService.InsertUser(userInfo);

                Assert.True(userInfo.UserId > 0);

                userService.DeleteUser(userInfo.UserId);

                var updatedUserInfo = userService.GetUser(userInfo.UserId);

                Assert.Null(updatedUserInfo);
            }
        }

        [Fact]
        public void GetWUserList()
        {
            using (var scopeProvider = this.serviceProvider.CreateScope())
            {
                var userService = scopeProvider.ServiceProvider.GetRequiredService<ICommonUserService>();

                var wlist = userService.GetUserList(null, Domain.Models.UserGender.W).ToList();

                Assert.True(wlist.All(o => o.UserGender == Domain.Models.UserGender.W));
            }
        }
    }
}

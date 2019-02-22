using Dapper;
using Insql.Tests.Domain.Contexts;
using Insql.Tests.Domain.Models;
using Insql.Tests.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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

            this.serviceCollection.AddInsql(builder =>
            {

            });

            this.serviceCollection.AddInsqlDbContext<UserDbContext>(options =>
            {
                options.UseSqlite("Data Source= ./insql.tests.db");
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

        [Fact]
        public void InsertUpdateGetUser()
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
                var userService = scopeProvider.ServiceProvider.GetRequiredService<IUserService>();

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
                var userService = scopeProvider.ServiceProvider.GetRequiredService<IUserService>();

                var wlist = userService.GetUserList(null, Domain.Models.UserGender.W).ToList();

                Assert.True(wlist.All(o => o.UserGender == Domain.Models.UserGender.W));
            }
        }

        [Fact]
        public void GetUserInList()
        {
            using (var scopeProvider = this.serviceProvider.CreateScope())
            {
                var userService = scopeProvider.ServiceProvider.GetRequiredService<IUserService>();

                userService.InsertUserList(new List<UserInfo>
                {
                    new UserInfo
                    {
                         UserName = "in1", UserGender = UserGender.M
                    },
                    new UserInfo
                    {
                         UserName = "in2",
                    }
                    ,
                    new UserInfo
                    {
                         UserName = "in3", UserGender = UserGender.M
                    }
                });

                var wlist = userService.GetUserInList(new string[] { "in1", "in2" });

                Assert.True(wlist.Count() > 0);
            }
        }

        [Fact]
        public void GetMapUser()
        {
            using (var scopeProvider = this.serviceProvider.CreateScope())
            {
                var userDbContext = scopeProvider.ServiceProvider.GetRequiredService<UserDbContext>();

                var userInfo = new Domain.Models.UserInfo
                {
                    UserName = new Random().Next().ToString(),
                    UserGender = Domain.Models.UserGender.W
                };

                userDbContext.InsertUser(userInfo);

                Assert.True(userInfo.UserId > 0);

                var selectUserInfo = userDbContext.GetMapUser(userInfo.UserId);

                Assert.True(selectUserInfo.MUserId == userInfo.UserId);
                Assert.True(selectUserInfo.MUserName == userInfo.UserName);
                Assert.True(selectUserInfo.user_gender == null);
            }
        }
    }
}

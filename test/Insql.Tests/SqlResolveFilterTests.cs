using Dapper;
using Insql.Tests.Domain.Contexts;
using Insql.Tests.Domain.Services;
using Insql.Tests.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace Insql.Tests
{
    public class SqlResolveFilterTests : IDisposable
    {
        private readonly IServiceCollection serviceCollection;
        private readonly IServiceProvider serviceProvider;

        public SqlResolveFilterTests()
        {
            this.serviceCollection = new ServiceCollection();

            this.serviceCollection.AddLogging(builder=> 
            {
                builder.SetMinimumLevel(LogLevel.Information);

                builder.AddConsole(options=> 
                {
                    
                });
            });

            this.serviceCollection.AddInsql(builder =>
            {
                builder.AddResolveFilter<LogResolveFilter>();
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
        public void InsertUserLog()
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
    }
}

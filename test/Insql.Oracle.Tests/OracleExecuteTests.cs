using Insql.Tests.OracleDomain;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Insql.Tests
{
    public class OracleExecuteTests : IDisposable
    {
        private readonly IServiceCollection serviceCollection;
        private readonly IServiceProvider serviceProvider;

        public OracleExecuteTests()
        {
            this.serviceCollection = new ServiceCollection();

            this.serviceCollection.AddInsql();

            this.serviceCollection.AddScoped<IOracleUserService, OracleUserService>();

            this.serviceProvider = this.serviceCollection.BuildServiceProvider();
        }

        public void Dispose()
        {
            if (this.serviceProvider is IDisposable dis)
            {
                dis.Dispose();
            }
        }

        public void InsertUser()
        {
            using (var scopeProvider = this.serviceProvider.CreateScope())
            {
                var userService = scopeProvider.ServiceProvider.GetRequiredService<IOracleUserService>();

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

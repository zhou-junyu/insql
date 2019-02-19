using Insql.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Insql.Tests
{
    public class RepeatSectionElementTests : IDisposable
    {
        private readonly IServiceCollection serviceCollection;
        private readonly IServiceProvider serviceProvider;

        public RepeatSectionElementTests()
        {
            this.serviceCollection = new ServiceCollection();

            this.serviceCollection.AddInsql();

            this.serviceProvider = this.serviceCollection.BuildServiceProvider();
        }

        public void Dispose()
        {
            if (this.serviceProvider is IDisposable dis)
            {
                dis.Dispose();
            }
        }

        [Fact]
        public void RepeatIn()
        {
            using (var scopeProvider = this.serviceProvider.CreateScope())
            {
                var sqlResolver = scopeProvider.ServiceProvider.GetRequiredService<ISqlResolver<RepeatSectionElementTests>>();

                var resolveResult = sqlResolver.Resolve(nameof(RepeatIn), new { userIdList = new string[] { "Tom", "Jerry" } });


                Assert.Equal("select * from user_info where user_id in (@userIdList1,@userIdList2)", resolveResult.Sql);

                Assert.True(resolveResult.Param.ContainsKey("userIdList1"));
                Assert.True(resolveResult.Param.ContainsKey("userIdList2"));
                Assert.False(resolveResult.Param.ContainsKey("userIdList"));
            }
        }

        [Fact]
        public void RepeatInNull()
        {
            using (var scopeProvider = this.serviceProvider.CreateScope())
            {
                var sqlResolver = scopeProvider.ServiceProvider.GetRequiredService<ISqlResolver<RepeatSectionElementTests>>();

                var resolveResult = sqlResolver.Resolve(nameof(RepeatInNull), new { userIdList = new string[] { } });

                Assert.Equal("select * from user_info", resolveResult.Sql);

                Assert.True(resolveResult.Param.ContainsKey("userIdList"));
            }
        }
    }
}

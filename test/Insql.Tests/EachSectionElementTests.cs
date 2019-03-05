using Insql.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Insql.Tests
{
    public class EachSectionElementTests : IDisposable
    {
        private readonly IServiceCollection serviceCollection;
        private readonly IServiceProvider serviceProvider;

        public EachSectionElementTests()
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
        public void EachIn()
        {
            using (var scopeProvider = this.serviceProvider.CreateScope())
            {
                var sqlResolver = scopeProvider.ServiceProvider.GetRequiredService<IInsqlResolver<EachSectionElementTests>>();

                var resolveResult = sqlResolver.Resolve(nameof(EachIn), new { userIdList = new string[] { "Tom", "Jerry" } });

                Assert.Equal("select * from user_info where user_id in (@userIdList1,@userIdList2)", resolveResult.Sql);

                Assert.True(resolveResult.Param.ContainsKey("userIdList1"));
                Assert.True(resolveResult.Param.ContainsKey("userIdList2"));
                Assert.False(resolveResult.Param.ContainsKey("userIdList"));
            }
        }

        [Fact]
        public void EachInNull()
        {
            using (var scopeProvider = this.serviceProvider.CreateScope())
            {
                var sqlResolver = scopeProvider.ServiceProvider.GetRequiredService<IInsqlResolver<EachSectionElementTests>>();

                var resolveResult = sqlResolver.Resolve(nameof(EachInNull), new { userIdList = new string[] { } });

                Assert.Equal("select * from user_info", resolveResult.Sql);

                Assert.True(resolveResult.Param.ContainsKey("userIdList"));
            }
        }

        [Fact]
        public void CDATATest()
        {
            using (var scopeProvider = this.serviceProvider.CreateScope())
            {
                var sqlResolver = scopeProvider.ServiceProvider.GetRequiredService<IInsqlResolver<EachSectionElementTests>>();

                var resolveResult = sqlResolver.Resolve(nameof(CDATATest));

                Assert.Equal("select * from user_info where create_time >= now()", resolveResult.Sql);
            }
        }

    }
}

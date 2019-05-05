using Insql.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Insql.Tests
{
   public class ElementDescriptorTests : TestsBase
   {
      [Fact]
      public void EachIn()
      {
         using (var scopeProvider = this.GlobalServiceProvider.CreateScope())
         {
            var sqlResolver = scopeProvider.ServiceProvider.GetRequiredService<IInsqlResolver<ElementDescriptorTests>>();

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
         using (var scopeProvider = this.GlobalServiceProvider.CreateScope())
         {
            var sqlResolver = scopeProvider.ServiceProvider.GetRequiredService<IInsqlResolver<ElementDescriptorTests>>();

            var resolveResult = sqlResolver.Resolve(nameof(EachInNull), new { userIdList = new string[] { } });

            Assert.Equal("select * from user_info", resolveResult.Sql);

            Assert.True(resolveResult.Param.ContainsKey("userIdList"));
         }
      }

      [Fact]
      public void CDATATest()
      {
         using (var scopeProvider = this.GlobalServiceProvider.CreateScope())
         {
            var sqlResolver = scopeProvider.ServiceProvider.GetRequiredService<IInsqlResolver<ElementDescriptorTests>>();

            var resolveResult = sqlResolver.Resolve(nameof(CDATATest));

            Assert.Equal("select * from user_info where create_time >= now()", resolveResult.Sql);
         }
      }

      [Fact]
      public void RawValueTest()
      {
         using (var scopeProvider = this.GlobalServiceProvider.CreateScope())
         {
            var sqlResolver = scopeProvider.ServiceProvider.GetRequiredService<IInsqlResolver<ElementDescriptorTests>>();

            var resolveResult = sqlResolver.Resolve(nameof(RawValueTest), new { orderBy = "create_time" });

            Assert.Equal("select * from user_info order by create_time", resolveResult.Sql);
         }
      }
   }
}

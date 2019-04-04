using Insql.Mappers;
using Insql.Models.ModelOne;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;

namespace Insql.Tests
{
    public class XmlTests : TestsBase
    {
        [Fact]
        public void EnabledTest()
        {
            var serviceProvider = this.CreateServiceProvider(builder =>
            {
                builder.AddMapper(options =>
                {
                });
            });

            using (serviceProvider)
            {
                using (var serviceScope = serviceProvider.CreateScope())
                {
                    var insqlModel = serviceScope.ServiceProvider.GetRequiredService<IInsqlModel>();

                    var entityMap = insqlModel.FindMap(typeof(FluentModelInfo));

                    //table
                    Assert.NotNull(entityMap);

                    Assert.Equal("annotation_model_info", entityMap.Table);

                    //id
                    var idMap = entityMap.Properties.SingleOrDefault(o => o.PropertyInfo.Name == "Id");

                    Assert.NotNull(idMap);
                    Assert.Equal("id", idMap.ColumnName);
                    Assert.True(idMap.IsKey);
                    Assert.True(idMap.IsIdentity);
                    Assert.False(idMap.IsIgnored);

                    //name
                    var nameMap = entityMap.Properties.SingleOrDefault(o => o.PropertyInfo.Name == "Name");

                    Assert.NotNull(nameMap);
                    Assert.Equal("name", nameMap.ColumnName);
                    Assert.False(nameMap.IsKey);
                    Assert.False(nameMap.IsIdentity);
                    Assert.False(nameMap.IsIgnored);

                    //size
                    var sizeMap = entityMap.Properties.SingleOrDefault(o => o.PropertyInfo.Name == "Size");

                    Assert.NotNull(sizeMap);
                    Assert.Equal("Size", sizeMap.ColumnName);
                    Assert.False(sizeMap.IsKey);
                    Assert.False(sizeMap.IsIdentity);
                    Assert.False(sizeMap.IsIgnored);

                    //extra
                    var extraMap = entityMap.Properties.SingleOrDefault(o => o.PropertyInfo.Name == "Extra");

                    Assert.Null(extraMap);
                }
            }
        }

        [Fact]
        public void DisableTest()
        {
            var serviceProvider = this.CreateServiceProvider(builder =>
            {
                builder.AddMapper(options =>
                {
                    options.DisableXmlMapScan();
                });
            });

            using (serviceProvider)
            {
                using (var serviceScope = serviceProvider.CreateScope())
                {
                    var insqlModel = serviceScope.ServiceProvider.GetRequiredService<IInsqlModel>();

                    var entityMap = insqlModel.FindMap(typeof(FluentModelInfo));

                    //table
                    Assert.Null(entityMap);
                }
            }
        }
    }
}

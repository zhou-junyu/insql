using Insql.Resolvers.Scripts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace Insql.Tests
{
    public class JavascriptCodeResolverTests : IDisposable
    {
        public void Dispose()
        {
        }

        [Fact]
        public void Basic()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddInsql(builder =>
             {
             });

            using (var serviceProvider = serviceCollection.BuildServiceProvider())
            {
                using (var scopeProvider = serviceProvider.CreateScope())
                {
                    var resolver = scopeProvider.ServiceProvider.GetRequiredService<IInsqlScriptResolver>();

                    var result = (bool)resolver.Resolve(TypeCode.Boolean, " userId != null ", new Dictionary<string, object>
                    {
                        { "userId","aa" }
                    });

                    Assert.True(result);

                    result = (bool)resolver.Resolve(TypeCode.Boolean, " userId == null ", new Dictionary<string, object>
                    {
                        { "userId",null }
                    });

                    Assert.True(result);

                    result = (bool)resolver.Resolve(TypeCode.Boolean, " userId == null ", new Dictionary<string, object>
                    {
                        { "userId","" }
                    });

                    Assert.False(result);
                }
            }
        }

        [Fact]
        public void ReplaceOperator()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddInsql(builder =>
             {
             });

            using (var serviceProvider = serviceCollection.BuildServiceProvider())
            {
                using (var scopeProvider = serviceProvider.CreateScope())
                {
                    var resolver = scopeProvider.ServiceProvider.GetRequiredService<IInsqlScriptResolver>();

                    var code = " userId != null and userId == 'aa' ";

                    var result = (bool)resolver.Resolve(TypeCode.Boolean, code, new Dictionary<string, object>
                    {
                        { "userId","aa" }
                    });

                    Assert.True(result);

                    code = " userId == null or userId != 'aa' or userId == \" \\\" ' and sdfsssdf '\" or userId gte \" s or ' s \" or userId eq 'aa' ";

                    result = (bool)resolver.Resolve(TypeCode.Boolean, code, new Dictionary<string, object>
                    {
                        { "userId","aa" }
                    });

                    Assert.True(result);

                    code = " userId != null and userId eq '\"' ";

                    result = (bool)resolver.Resolve(TypeCode.Boolean, code, new Dictionary<string, object>
                    {
                        { "userId","\"" }
                    });

                    Assert.True(result);

                    code = " userId != null and userId == '\\\'' ";

                    result = (bool)resolver.Resolve(TypeCode.Boolean, code, new Dictionary<string, object>
                    {
                        { "userId","'" }
                    });

                    Assert.True(result);
                }
            }
        }

        [Fact]
        public void NotReplaceOperator()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddInsql(builder =>
             {
                 builder.AddDefaultScriptResolver(options =>
                 {
                     options.IsConvertOperator = false;
                 });
             });

            using (var serviceProvider = serviceCollection.BuildServiceProvider())
            {
                using (var scopeProvider = serviceProvider.CreateScope())
                {
                    var resolver = scopeProvider.ServiceProvider.GetRequiredService<IInsqlScriptResolver>();

                    var code = @"var and = 'aa'; userId == and ";

                    var result = (bool)resolver.Resolve(TypeCode.Boolean, code, new Dictionary<string, object>
                    {
                        { "userId","aa" }
                    });

                    Assert.True(result);
                }
            }
        }

        [Fact]
        public void DataParameter()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddInsql(builder =>
            {
                builder.AddDefaultScriptResolver(options =>
                {
                    options.IsConvertOperator = false;
                });
            });

            using (var serviceProvider = serviceCollection.BuildServiceProvider())
            {
                using (var scopeProvider = serviceProvider.CreateScope())
                {
                    var resolver = scopeProvider.ServiceProvider.GetRequiredService<IInsqlScriptResolver>();

                    var code = @"userId !=null && userName != null";

                    var result = (bool)resolver.Resolve(TypeCode.Boolean, code, new Dictionary<string, object>
                    {
                        { "userId",new System.Data.SqlClient.SqlParameter("userId","asdf") },
                         { "userName","11"},
                    });

                    Assert.True(result);
                }
            }
        }
    }
}

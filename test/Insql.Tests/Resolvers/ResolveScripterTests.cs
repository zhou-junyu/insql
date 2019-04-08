using Insql.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace Insql.Tests
{
    public class ResolveScripterTests : TestsBase
    {
        [Fact]
        public void Basic()
        {
            using (var serviceProvider = this.CreateServiceProvider())
            {
                using (var scopeProvider = serviceProvider.CreateScope())
                {
                    var resolver = scopeProvider.ServiceProvider.GetRequiredService<IInsqlResolveScripter>();

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
            using (var serviceProvider = this.CreateServiceProvider())
            {
                using (var scopeProvider = serviceProvider.CreateScope())
                {
                    var resolver = scopeProvider.ServiceProvider.GetRequiredService<IInsqlResolveScripter>();

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
            using (var serviceProvider = this.CreateServiceProvider(builder =>
            {
                builder.AddResolver(configure =>
                {
                    configure.AddScripter(options =>
                    {
                        options.IsConvertOperator = false;
                    });
                });
            }))
            {
                using (var scopeProvider = serviceProvider.CreateScope())
                {
                    var resolver = scopeProvider.ServiceProvider.GetRequiredService<IInsqlResolveScripter>();

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
        public void ExcludeOperator()
        {
            using (var serviceProvider = this.CreateServiceProvider(builder =>
            {
                builder.AddResolver(configure =>
                {
                    configure.AddScripter(options =>
                    {
                        options.ExcludeOperators = new string[] { "and" };
                    });
                });
            }))
            {
                using (var scopeProvider = serviceProvider.CreateScope())
                {
                    var resolver = scopeProvider.ServiceProvider.GetRequiredService<IInsqlResolveScripter>();

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
            using (var serviceProvider = this.CreateServiceProvider(builder =>
            {
                builder.AddResolver(configure =>
                {
                    configure.AddScripter(options =>
                    {
                        options.IsConvertOperator = false;
                    });
                });
            }))
            {
                using (var scopeProvider = serviceProvider.CreateScope())
                {
                    var resolver = scopeProvider.ServiceProvider.GetRequiredService<IInsqlResolveScripter>();

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

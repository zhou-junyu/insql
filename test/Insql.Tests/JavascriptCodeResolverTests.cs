using Insql.Resolvers.Codes;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using Xunit;

namespace Insql.Tests
{
    public class JavascriptCodeResolverTests
    {
        [Fact]
        public void Test1()
        {
            var code = " userId != null ";

            var resolver = new JavaScriptCodeResolver(Options.Create(new JavascriptCodeResolverOptions { }));

            var result = (bool)resolver.Resolve(typeof(bool), code, new Dictionary<string, object>
            {
                { "userId","aa" }
            });

            Assert.True(result);

            result = (bool)resolver.Resolve(typeof(bool), code, new Dictionary<string, object>
            {
                { "userId",null }
            });

            Assert.False(result);
        }

        [Fact]
        public void Test2()
        {
            var code = " userId != null and userId == 'aa' ";

            var resolver = new JavaScriptCodeResolver(Options.Create(new JavascriptCodeResolverOptions { }));

            var result = (bool)resolver.Resolve(typeof(bool), code, new Dictionary<string, object>
            {
                { "userId","aa" }
            });

            Assert.True(result);
        }

        [Fact]
        public void Test3()
        {
            var code = " userId == null or userId == 'aa' or userId != \" \\\" ' and sdfsssdf '\" or userId gte \" s or ' s \" ";

            var resolver = new JavaScriptCodeResolver(Options.Create(new JavascriptCodeResolverOptions { }));

            var result = (bool)resolver.Resolve(typeof(bool), code, new Dictionary<string, object>
            {
                { "userId","aa" }
            });

            Assert.True(result);
        }
    }
}

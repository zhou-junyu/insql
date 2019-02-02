using Insql.Resolvers.Codes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
            var resolver = new JavaScriptCodeResolver(Options.Create(new JavascriptCodeResolverOptions()));

            var result = (bool)resolver.Resolve(typeof(bool), " userId != null ", new Dictionary<string, object>
            {
                { "userId","aa" }
            });

            Assert.True(result);

            result = (bool)resolver.Resolve(typeof(bool), " userId == null ", new Dictionary<string, object>
            {
                { "userId",null }
            });

            Assert.True(result);

            result = (bool)resolver.Resolve(typeof(bool), " userId == null ", new Dictionary<string, object>
            {
                { "userId","" }
            });

            Assert.False(result);
        }
        
        [Fact]
        public void ReplaceOperator()
        {
            var resolver = new JavaScriptCodeResolver(Options.Create(new JavascriptCodeResolverOptions { }));

            var code = " userId != null and userId == 'aa' ";

            var result = (bool)resolver.Resolve(typeof(bool), code, new Dictionary<string, object>
            {
                { "userId","aa" }
            });

            Assert.True(result);

            code = " userId == null or userId != 'aa' or userId == \" \\\" ' and sdfsssdf '\" or userId gte \" s or ' s \" or userId eq 'aa' ";

            result = (bool)resolver.Resolve(typeof(bool), code, new Dictionary<string, object>
            {
                { "userId","aa" }
            });

            Assert.True(result);

            code = " userId != null and userId eq '\"' ";

            result = (bool)resolver.Resolve(typeof(bool), code, new Dictionary<string, object>
            {
                { "userId","\"" }
            });

            Assert.True(result);

            code = " userId != null and userId == '\\\'' ";

            result = (bool)resolver.Resolve(typeof(bool), code, new Dictionary<string, object>
            {
                { "userId","'" }
            });

            Assert.True(result);
        }


        [Fact]
        public void NotReplaceOperator()
        {
            var resolver = new JavaScriptCodeResolver(Options.Create(new JavascriptCodeResolverOptions
            {
                IsReplaceOperator = false
            }));

            var code = @"var and = 'aa'; userId == and ";

            var result = (bool)resolver.Resolve(typeof(bool), code, new Dictionary<string, object>
            {
                { "userId","aa" }
            });

            Assert.True(result);
        }
    }
}

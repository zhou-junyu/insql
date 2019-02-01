using Jint;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Insql.Resolvers.Codes
{
    public class JavaScriptCodeResolver : IInsqlCodeResolver
    {
        private readonly Regex clearRegex;
        private readonly Regex operatorRegex;
        private readonly ConcurrentDictionary<string, string> codeCaches;

        private readonly IOptions<JavascriptCodeResolverOptions> options;

        public JavaScriptCodeResolver(IOptions<JavascriptCodeResolverOptions> options)
        {
            this.options = options;

            this.clearRegex = new Regex("(['\"]).*?[^\\\\]\\1");
            this.operatorRegex = new Regex("\\s+(and|or|gt|gte|lt|lte|eq|neq)\\s+");

            this.codeCaches = new ConcurrentDictionary<string, string>();
        }

        public void Dispose()
        {
        }

        public object Resolve(Type type, string code, IDictionary<string, object> param)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code));
            }
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }
            var optionsValue = this.options.Value;

            if (optionsValue.IsReplaceOperator)
            {
                code = this.codeCaches.GetOrAdd(code.GetHashCode().ToString(), (key) => this.ReplaceOperator(code));
            }

            var engine = new Engine(options =>
            {
                options.DebugMode(false);

                if (optionsValue.IsConvertEnum)
                {
                    options.AddObjectConverter(JavaScriptEnumConverter.Instance);
                }
            });

            foreach (var item in param)
            {
                engine.SetValue(item.Key, item.Value);
            };

            engine.Execute(code);

            var value = engine.GetCompletionValue();

            var result = value.ToObject();

            if (result == null)
            {
                return null;
            }

            return Convert.ChangeType(result, type);
        }

        private string ReplaceOperator(string code)
        {
            var clearMatchs = clearRegex.Matches(code);

            return this.operatorRegex.Replace(code, match =>
            {
                if (!match.Success)
                {
                    return match.Value;
                }

                var endIndex = match.Index + match.Length - 1;

                if (clearMatchs.Cast<Match>().Any(cmatch =>
                {
                    var cendIndex = cmatch.Index + cmatch.Length - 1;

                    return match.Index > cmatch.Index && endIndex < cendIndex;
                }))
                {
                    return match.Value;
                }

                return match.Value
                .Replace(" and ", " && ")
                .Replace(" or ", " || ")
                .Replace(" gt ", " > ")
                .Replace(" gte ", " >= ")
                .Replace(" lt ", " < ")
                .Replace(" lte ", " <= ")
                .Replace(" eq ", " == ")
                .Replace(" neq ", " != ");
            });
        }
    }
}

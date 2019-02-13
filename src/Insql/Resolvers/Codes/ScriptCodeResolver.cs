using Jint;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Insql.Resolvers.Codes
{
    public class ScriptCodeResolver : IInsqlCodeResolver
    {
        private readonly Regex excludeRegex;
        private readonly Regex operatorRegex;
        private readonly ConcurrentDictionary<string, string> codeCaches;
        private readonly Dictionary<string, string> operatorMappings;

        private readonly IOptions<ScriptCodeResolverOptions> options;

        public ScriptCodeResolver(IOptions<ScriptCodeResolverOptions> options)
        {
            this.options = options;

            this.codeCaches = new ConcurrentDictionary<string, string>();

            this.operatorMappings = new Dictionary<string, string>
            {
                { "and","&&" },
                { "or","||" },
                { "gt",">" },
                { "gte",">=" },
                { "lt","<" },
                { "lte","<=" },
                { "eq","==" },
                { "neq","!=" },
            };

            this.excludeRegex = new Regex("(['\"]).*?[^\\\\]\\1");
            this.operatorRegex = new Regex($"\\s+({string.Join("|", this.operatorMappings.Keys)})\\s+");
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
                code = this.codeCaches.GetOrAdd(code, (key) => this.ReplaceOperator(code));
            }

            var engine = new Engine(options =>
            {
                options.DebugMode(false);

                if (optionsValue.IsConvertEnum)
                {
                    options.AddObjectConverter(ScriptEnumConverter.Instance);
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
            var excludeMatchs = excludeRegex.Matches(code);

            return this.operatorRegex.Replace(code, match =>
            {
                if (!match.Success)
                {
                    return match.Value;
                }

                var endIndex = match.Index + match.Length - 1;

                if (excludeMatchs.Cast<Match>().Any(cmatch =>
                {
                    var cendIndex = cmatch.Index + cmatch.Length - 1;

                    return match.Index > cmatch.Index && endIndex < cendIndex;
                }))
                {
                    return match.Value;
                }

                var operatorGroup = match.Groups.Count > 1 ? match.Groups[1] : null;

                if (operatorGroup == null || !operatorGroup.Success)
                {
                    return match.Value;
                }

                if (!this.operatorMappings.TryGetValue(operatorGroup.Value, out string operatorValue))
                {
                    return match.Value;
                }

                return $"{match.Value.Substring(0, operatorGroup.Index - match.Index)}" +
                    $"{operatorValue}" +
                    $"{match.Value.Substring(operatorGroup.Index - match.Index + operatorGroup.Length)}";
            });
        }
    }
}

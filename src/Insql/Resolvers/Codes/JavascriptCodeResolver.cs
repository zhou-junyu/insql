using Jint;
using Jint.Native;
using Jint.Runtime.Interop;
using System;
using System.Collections.Generic;

namespace Insql.Resolvers.Codes
{
    internal class JavaScriptCodeResolver : IInsqlCodeResolver
    {
        public void Dispose()
        {
        }

        public object Resolve(Type type, string code, IDictionary<string, object> param)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(code);
            }
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var codeString = this.ReplaceCodeOperator(code);

            var engine = new Engine(options =>
            {
                options.DebugMode(false);
                options.AddObjectConverter(JavaScriptEnumConverter.Instance);
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

        private string ReplaceCodeOperator(string code)
        {
            return code
                .Replace(" and ", " && ")
                .Replace(" or ", " || ")
                .Replace(" gt ", " > ")
                .Replace(" gte ", " >= ")
                .Replace(" lt ", " < ")
                .Replace(" lte ", " <= ")
                .Replace(" eq ", " == ")
                .Replace(" neq ", " != ");
        }
    }

    internal class JavaScriptEnumConverter : IObjectConverter
    {
        public static JavaScriptEnumConverter Instance = new JavaScriptEnumConverter();

        public bool TryConvert(object value, out JsValue result)
        {
            if (value == null)
            {
                result = JsValue.Null; return false;
            }

            if (value.GetType().IsEnum)
            {
                result = new JsValue(value.ToString()); return true;
            }

            result = JsValue.Null; return false;
        }
    }
}

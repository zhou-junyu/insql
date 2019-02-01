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
